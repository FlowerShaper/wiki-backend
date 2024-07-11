using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes;

public class GetFeaturedArticleRoute : IWikiAPIRoute
{
    public string RoutePath => "/featured";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        var value = DynamicsHelper.Get(DynamicDataType.FeaturedPost);

        if (string.IsNullOrWhiteSpace(value))
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        var article = ArticleHelper.GetArticle(value, Language.en);

        if (article is null)
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, article);
    }
}
