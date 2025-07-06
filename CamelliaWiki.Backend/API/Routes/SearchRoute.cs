using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Utils;
using Midori.Networking;
using Midori.Searching;

namespace CamelliaWiki.Backend.API.Routes;

public class SearchRoute : IWikiAPIRoute
{
    public string RoutePath => "/search";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringQuery("query", out var query))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "Please provide a search query.");
            return;
        }

        LanguageUtils.TryParse(interaction.GetStringQuery("lang"), out var language);

        var filter = new SearchFilter<Article>(query);
        var articles = filter.Filter(ArticleHelper.GetAllUniqueArticles(language));
        await interaction.Reply(HttpStatusCode.OK, articles);
    }
}
