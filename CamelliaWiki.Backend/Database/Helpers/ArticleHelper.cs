using CamelliaWiki.Backend.Components;
using CamelliaWiki.Backend.Components.Articles;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class ArticleHelper
{
    private static IMongoCollection<Article> collection => MongoDatabase.GetCollection<Article>("articles");

    public static void AddArticle(Article article) => collection.InsertOne(article);

    public static Article? GetArticle(string path, Language lang)
    {
        var id = $"{path}:{lang}";
        var article = collection.Find(a => a.ID == id).FirstOrDefault();

        // fallback to English if the article is not found
        if (article is null && lang != Language.en)
            article = collection.Find(a => a.ID == $"{path}:en").FirstOrDefault();

        return article;
    }

    public static void Wipe() => collection.DeleteMany(FilterDefinition<Article>.Empty);
}
