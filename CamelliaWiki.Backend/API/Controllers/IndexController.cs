using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Utils;
using Midori.API.Attributes;
using Midori.API.Components;
using Midori.Searching;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller]
public class IndexController
{
    private readonly DatabaseContext database;

    public IndexController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/cdn")]
    public APIReturn<Stream> Asset([Source(ParameterSource.Query)] string path)
    {
        path = path.TrimStart('/');
        var fs = Path.Combine(Program.Config.DataDirectory, "_assets", path);

        if (!File.Exists(fs))
            return Returns.NotFound();

        return File.OpenRead(fs);
    }

    [HttpRoute("/home")]
    public APIReturn<Article[][]> HomeArticles()
    {
        var dyn = database.Dynamics.Find(DynamicDataType.HomePosts);
        if (dyn is null) return Array.Empty<Article[]>();

        var categories = dyn.Value.Split("::");
        return categories.Select(c => c.Split(';')
                                       .Select(a => database.FindArticle(a, ArticleLanguage.en))
                                       .OfType<Article>().ToArray()
        ).ToArray();
    }

    [HttpRoute("/search")]
    public APIReturn<List<Article>> Search([Source(ParameterSource.Query)] string query, [Source(ParameterSource.Query)] string? lang)
    {
        query = query.ToLowerInvariant();
        _ = LanguageUtils.TryParse(lang, out var language);

        var articleFilter = new SearchFilter<Article>(query);
        var articles = articleFilter.Filter(database.Articles.GetAllUnique(language).Select(a =>
        {
            database.Entry(a).Reference(r => r.Metadata).Load();
            return a;
        }));

        // var discFilter = new SearchFilter<IDiscographySearchable>(query);
        // var discography = discFilter.Filter(DiscographyHelper.Searchable);

        /*
         foreach (var searchable in discography)
           {
               var track = searchable is DiscographyTrack;

               articles.Add(new Article
               {
                   ID = $"/discography/{(track ? "tracks" : "albums")}/{searchable.ID}:en",
                   Metadata = new ArticleMetadata
                   {
                       Title = searchable.Title,
                       Description = track ? "Track" : "Album",
                       Image = searchable.ImageUrl ?? string.Empty
                   }
               });
           }
         */

        return articles;
    }
}
