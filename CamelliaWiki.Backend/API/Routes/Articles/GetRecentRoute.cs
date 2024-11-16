using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models.Articles;

namespace CamelliaWiki.Backend.API.Routes.Articles;

public class GetRecentRoute : IWikiAPIRoute
{
    public string RoutePath => "/articles/recent";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        var type = interaction.GetStringQuery("type") ?? "all";

        var count = interaction.GetIntQuery("count") ?? 5;
        count = Math.Clamp(count, 1, 8);

        var articles = ArticleHelper.All.OrderDescending().ToList();

        switch (type)
        {
            case "all": // stays the same
                break;

            case "news":
                articles = articles.Where(x => x.Metadata.Type == ArticleType.News).ToList();
                break;
        }

        await interaction.Reply(HttpStatusCode.OK, articles.Take(count));
    }
}
