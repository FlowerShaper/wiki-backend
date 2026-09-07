using System.Diagnostics.CodeAnalysis;
using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using Microsoft.EntityFrameworkCore;

namespace CamelliaWiki.Backend.Database.Extensions;

public static class DatabaseArticleExtensions
{
    public static List<Article> GetAllUnique(this DbSet<Article> set, ArticleLanguage lang) => [.. set.Where(x => x.Language == lang)];

    public static bool TryGetAlias(this DbSet<ArticleAlias> set, string alias, [NotNullWhen(true)] out string? article)
    {
        var a = set.Find(alias);
        article = a?.Article;
        return !string.IsNullOrEmpty(article);
    }

    public static Article? FindArticle(this DatabaseContext db, string path, ArticleLanguage lang)
    {
        if (path == "/random")
            return getRandom(db, lang);

        if (db.Aliases.TryGetAlias(path, out var articleID))
            return db.FindArticle(articleID, lang);

        var article = db.Articles.Find(path, lang);

        // fallback to English if the article is not found
        if (article is null && lang != ArticleLanguage.en)
            article = db.Articles.Find(path, ArticleLanguage.en);

        if (article != null)
            db.Entry(article).Reference(x => x.Metadata).Load();

        return article;
    }

    private static Article? getRandom(DatabaseContext db, ArticleLanguage lang)
    {
        var names = db.Articles.Select(a => a.ID).Distinct().ToList();
        var random = new Random().Next(0, names.Count);
        return db.FindArticle(names[random], lang);
    }
}
