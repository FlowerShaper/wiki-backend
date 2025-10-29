using System.Diagnostics;
using System.Text.RegularExpressions;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Models.Discography;
using CamelliaWiki.Backend.Utils;
using Midori.Logging;

namespace CamelliaWiki.Backend.Processing;

public class DataProcessor
{
    private static readonly Regex metadata_regex = new(@"^---([\s\S]*?)---", RegexOptions.Multiline);
    private static Logger logger { get; } = Logger.GetLogger("MarkdownProcessor");

    private string dataDirectory { get; }

    private DataProcessor(string dataDirectory)
    {
        this.dataDirectory = dataDirectory.Replace('/', Path.DirectorySeparatorChar);
    }

    public static void Run(string path)
    {
        if (!Directory.Exists(path))
            return;

        var runner = new DataProcessor(path);
        runner.Start();
    }

    public void Start()
    {
        ArticleHelper.Wipe();
        DiscographyHelper.Wipe();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        processFolder(dataDirectory);

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
        var content = extractContent(md);

        var breadCrumbs = new List<Breadcrumb>();

        var pathSplit = folderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var title = metadata.GetValueOrDefault("title", pathSplit.Last().FormatToTitle());

        if (pathSplit.Length > 1)
        {
            // reconstruct the path every path
            // so that /a/b/c/d
            // becomes /a /a/b /a/b/c /a/b/c/d

            var currentPath = "";

            foreach (var path in pathSplit)
            {
                if (path == pathSplit.Last())
                    break;

                currentPath += $"/{path}";

                var art = ArticleHelper.GetArticle(currentPath, lang);

                breadCrumbs.Add(new Breadcrumb
                {
                    Name = art is not null ? art.Metadata.Title : path.FormatToTitle(),
                    Path = currentPath
                });
            }
        }

        breadCrumbs.Add(new Breadcrumb
        {
            Name = title,
            Path = folderPath
        });

        logger.Add($"    Breadcrumbs: {string.Join(" -> ", breadCrumbs.Select(x => x.Name))}");
        logger.Add($"    Parent Paths: {string.Join(" -> ", breadCrumbs.Select(x => x.Path))}");

        var article = new Article
        {
            ID = $"{folderPath}:{name}",
            Content = content,
            Breadcrumbs = breadCrumbs,
            Metadata = new ArticleMetadata
            {
                Title = title,
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
            }
        };

        ArticleHelper.AddArticle(article);
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
            album.Content = "> TODO: Add content.\n{: .caution }";

        album.ID = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
        DiscographyHelper.AddAlbum(album);

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
            track.Content = "> TODO: Add content.\n{: .caution }";

        track.ID = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();
        DiscographyHelper.AddTrack(track);

        logger.Add($"    Title: {track.Title} ({track.ID})");
    }

    #endregion
}
