using System.Diagnostics;
using System.Text.RegularExpressions;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Models.Discography;
using CamelliaWiki.Backend.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Midori.Logging;
using Midori.Utils;

namespace CamelliaWiki.Backend.Processing;

public class DataProcessor
{
    private static readonly Regex metadata_regex = new(@"^---([\s\S]*?)---", RegexOptions.Multiline);
    private static Logger logger { get; } = Logger.GetLogger("MarkdownProcessor");

    private readonly DatabaseContext database;
    private string dataDirectory = string.Empty;

    private readonly List<Article> articles = [];
    private readonly List<ArticleMetadata> articleMeta = [];
    private readonly List<DiscographyAlbum> albums = [];
    private readonly List<DiscographyTrack> tracks = [];

    public DataProcessor(DatabaseContext database)
    {
        this.database = database;
    }

    public void Start(string path)
    {
        dataDirectory = path.Replace('/', Path.DirectorySeparatorChar);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // database.Database.EnsureDeleted();
        database.Database.EnsureCreated();

        // remove old data
        database.Articles.ExecuteDelete();
        database.ArticleMeta.ExecuteDelete();
        database.Albums.ExecuteDelete();
        database.Tracks.ExecuteDelete();

        using (database.EditAndSave())
        {
            processFolder(dataDirectory);

            database.Articles.AddRange(articles);
            database.ArticleMeta.AddRange(articleMeta);
            database.Albums.AddRange(albums);
            database.Tracks.AddRange(tracks);
        }

        stopwatch.Stop();

        logger.Add($"Finished in {stopwatch.ElapsedMilliseconds}ms");
    }

    private void processFolder(string folder)
    {
        var name = Path.GetFileName(folder);
        if (name.StartsWith("__")) return;

        processFolderMarkdown(folder);
        processFolderData(folder);

        var subFolders = Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly);

        foreach (var subFolder in subFolders)
            processFolder(subFolder);
    }

    #region Markdown

    private void processFolderMarkdown(string folder)
    {
        var files = Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
            processMarkdownFile(file);
    }

    private void processMarkdownFile(string file)
    {
        var folderPath = Path.GetDirectoryName(file)!.Replace(dataDirectory, "");
        folderPath = folderPath.Replace(Path.DirectorySeparatorChar, '/');

        if (folderPath.StartsWith("/_data"))
            return;

        var name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();

        if (!LanguageUtils.TryParse(name, out var lang))
            return;

        logger.Add($"Processing {folderPath} ({name})");

        var md = File.ReadAllText(file);
        var metadata = extractMetadata(md);
        var content = extractContent(md).Trim();

        var article = new Article
        {
            ID = folderPath,
            Language = lang,
            Content = content
        };

        var meta = new ArticleMetadata
        {
            ID = article.ID,
            Language = article.Language,
            Title = metadata.GetValueOrDefault("title", folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries).Last().FormatToTitle()),
            Description = metadata.GetValueOrDefault("description", "No description provided."),
            Image = metadata.GetValueOrDefault("image", ""),
            Layout = metadata.GetValueOrDefault("layout", "article"),
            Type = metadata.GetValueOrDefault("type", "article") switch
            {
                "article" => ArticleType.Article,
                "news" => ArticleType.News,
                _ => ArticleType.Article,
            },
            Date = metadata.TryGetValue("date", out var value) ? parseDate(value) : 0
        };

        articles.Add(article);
        articleMeta.Add(meta);
    }

    private long parseDate(string date)
    {
        try
        {
            var dt = DateTime.Parse(date);
            return new DateTimeOffset(dt).ToUnixTimeSeconds();
        }
        catch
        {
            return 0;
        }
    }

    private static Dictionary<string, string> extractMetadata(string md)
    {
        var metadata = new Dictionary<string, string>();

        var match = metadata_regex.Match(md);

        if (!match.Success)
            return metadata;

        var metadataString = match.Groups[0].Value;
        var lines = metadataString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (var l in lines)
        {
            var line = l.Trim();

            if (line.StartsWith("---") || string.IsNullOrEmpty(line))
                continue;

            if (!line.Contains(':'))
            {
                var lastKey = metadata.Keys.Last();
                metadata[lastKey] += $" {line}";
                continue;
            }

            var parts = line.Split(':', 2);

            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim();
            var value = parts[1].Trim();

            metadata[key] = value;
        }

        return metadata;
    }

    private static string extractContent(string md)
    {
        var match = metadata_regex.Match(md);

        if (!match.Success)
            return md;

        var metadataString = match.Groups[0].Value;
        return md.Replace(metadataString, "");
    }

    #endregion

    #region Data

    private void processFolderData(string folder)
    {
        var files = Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
        {
            var relative = file.Replace(dataDirectory, "").Replace(Path.DirectorySeparatorChar, '/');

            if (relative.StartsWith("/_data/albums"))
                processAlbumData(file);
            else if (relative.StartsWith("/_data/tracks"))
                processTrackData(file);
            else
                logger.Add($"Unsure how to process data file: {relative}", LogLevel.Warning);
        }
    }

    private void processAlbumData(string file)
    {
        logger.Add($"Processing album {file}");
        var json = File.ReadAllText(file);
        var album = json.Deserialize<DiscographyAlbum>();

        if (album == null)
        {
            logger.Add($"Failed to deserialize album data from '{file}'!", LogLevel.Error);
            return;
        }

        var md = Path.ChangeExtension(file, "md");
        if (File.Exists(md)) album.Content = File.ReadAllText(md);

        if (string.IsNullOrWhiteSpace(album.Content))
            album.Content = "> [!NOTE]\n> TODO: Add content.";

        album.ID = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
        albums.Add(album);

        logger.Add($"    Title: {album.Title} ({album.ID})");
    }

    private void processTrackData(string file)
    {
        logger.Add($"Processing track {file}");
        var json = File.ReadAllText(file);
        var track = json.Deserialize<DiscographyTrack>();

        if (track == null)
        {
            logger.Add($"Failed to deserialize track data from '{file}'!", LogLevel.Error);
            return;
        }

        var md = Path.ChangeExtension(file, "md");
        if (File.Exists(md)) track.Content = File.ReadAllText(md);

        if (string.IsNullOrWhiteSpace(track.Content))
            track.Content = "> [!NOTE]\n> TODO: Add content.";

        track.ID = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
        tracks.Add(track);

        logger.Add($"    Title: {track.Title} ({track.ID})");
    }

    #endregion
}
