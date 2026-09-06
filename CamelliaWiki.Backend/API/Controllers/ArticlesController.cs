using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Utils;
using Midori.API.Attributes;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/articles")]
public class ArticlesController
{
    private readonly DatabaseContext database;

    public ArticlesController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/")]
    public APIReturn<Article> Get([Source(ParameterSource.Query)] string path, [Source(ParameterSource.Query)] string? lang)
    {
        _ = LanguageUtils.TryParse(lang, out var language);

        var article = database.FindArticle(path, language);
        if (article is null) return Returns.NotFound();

        // TODO: log to managers
        return article;
    }
}
