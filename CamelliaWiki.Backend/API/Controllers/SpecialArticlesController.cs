using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using Midori.API.Attributes;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller]
public class SpecialArticlesController
{
    private readonly DatabaseContext database;

    public SpecialArticlesController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/featured")]
    public APIReturn<Article> Featured()
    {
        // TODO: implement dynamics
        string? value = null;
        if (value is null) return Returns.NotFound();

        var article = database.FindArticle(value, ArticleLanguage.en);
        if (article is null) return Returns.NotFound();

        return article;
    }
}
