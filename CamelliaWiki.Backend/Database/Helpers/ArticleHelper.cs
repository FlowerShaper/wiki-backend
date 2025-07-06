using System.Diagnostics.CodeAnalysis;
using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class ArticleHelper
{
    private static IMongoCollection<Article> collection => MongoDatabase.GetCollection<Article>("articles");
    private static IMongoCollection<ArticleAlias> aliases => MongoDatabase.GetCollection<ArticleAlias>("aliases");

    public static List<Article> All => collection.Find(_ => true).ToList();
    public static List<Article> AllUnique => collection.Find(_ => true).ToList();

    public static void AddArticle(Article article) => collection.InsertOne(article);
    public static void AddAlias(string alias, string article) => aliases.InsertOne(new ArticleAlias { Alias = alias, Article = article });

    public static List<Article> GetAllUniqueArticles(Language lang)
    {
        var all = All;
        return all.GroupBy(a => a.Path)
                  .Select(g => g.FirstOrDefault(a => a.Language == lang) ?? g.FirstOrDefault(a => a.Language == Language.en))
                  .OfType<Article>()
                  .ToList();
    }

    public static bool TryGetAlias(string alias, [NotNullWhen(true)] out string? article)
    {
        var a = aliases.Find(a => a.Alias == alias).FirstOrDefault();
        article = a?.Article;
        return !string.IsNullOrEmpty(article);
    }

    public static Article? GetArticle(string path, Language lang)
    {
        if (path == "/random")
            return getRandom(lang);

        if (TryGetAlias(path, out var articleID))
            return GetArticle(articleID, lang);

        var id = $"{path}:{lang}";
        var article = collection.Find(a => a.ID == id).FirstOrDefault();

        // fallback to English if the article is not found
        if (article is null && lang != Language.en)
            article = collection.Find(a => a.ID == $"{path}:en").FirstOrDefault();

        return article;
    }

    private static Article? getRandom(Language lang)
    {
        var names = collection.Find(_ => true).ToList().Select(a => a.ID.Split(":").First()).Distinct().ToList();
        var random = new Random().Next(0, names.Count);
        return GetArticle(names[random], lang);
    }

    public static void Wipe() => collection.DeleteMany(_ => true);
}
