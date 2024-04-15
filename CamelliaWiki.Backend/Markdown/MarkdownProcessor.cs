using System.Text.RegularExpressions;
using CamelliaWiki.Backend.Components.Articles;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.Markdown;

public class MarkdownProcessor
{
    private static readonly Regex metadata_regex = new(@"^---([\s\S]*?)---", RegexOptions.Multiline);

    private string dataDirectory { get; }

    private MarkdownProcessor(string dataDirectory)
    {
        this.dataDirectory = dataDirectory.Replace('/', Path.DirectorySeparatorChar);
    }

    public static void Run(string path)
    {
        if (!Directory.Exists(path))
            return;

        var runner = new MarkdownProcessor(path);
        runner.Start();
    }

    public void Start()
    {
        ArticleHelper.Wipe();
        processFolder(dataDirectory);
    }

    private void processFolder(string folder)
    {
        var files = Directory.GetFiles(folder, "*.md", SearchOption.TopDirectoryOnly);
        var subFolders = Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly);

        foreach (var file in files)
            processFile(file);

        foreach (var subFolder in subFolders)
            processFolder(subFolder);
    }

    private void processFile(string file)
    {
        var folderPath = Path.GetDirectoryName(file)!.Replace(dataDirectory, "");
        folderPath = folderPath.Replace(Path.DirectorySeparatorChar, '/');

        var name = Path.GetFileNameWithoutExtension(file).ToLowerInvariant();

        if (!LanguageUtils.TryParse(name, out _))
            return;

        Logger.Log($"Processing {folderPath} ({name})");

        var md = File.ReadAllText(file);
        var metadata = extractMetadata(md);
        var content = extractContent(md);

        var article = new Article
        {
            ID = $"{folderPath}:{name}",
            Content = content,
            Metadata = new ArticleMetadata
            {
                Title = metadata.GetValueOrDefault("title", ""),
                Description = metadata.GetValueOrDefault("description", "No description provided."),
                Author = metadata.GetValueOrDefault("author", "Unknown"),
                Layout = metadata.GetValueOrDefault("layout", "article"),
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
}
