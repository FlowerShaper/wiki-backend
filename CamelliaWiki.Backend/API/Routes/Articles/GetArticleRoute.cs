using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Articles;

public class GetArticleRoute : IWikiAPIRoute
{
    public string RoutePath => "/articles";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringQuery("path", out var path))
            return;

        LanguageUtils.TryParse(interaction.GetStringQuery("lang"), out var language);

        var article = ArticleHelper.GetArticle(path, language);

        if (article is null)
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, article);
        Program.ViewManager.Log(interaction.RemoteIP, article.Path);
    }
}
