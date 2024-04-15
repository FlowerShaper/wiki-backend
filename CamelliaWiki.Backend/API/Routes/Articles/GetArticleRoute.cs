using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Articles;

public class GetArticleRoute : IAPIRoute
{
    public string Path => "/articles";
    public HttpMethod Method => HttpMethod.Get;
    public bool RequiresAuthentication => false;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringQuery("path", out var path))
            return;

        LanguageUtils.TryParse(interaction.GetStringQuery("lang"), out var language);

        var article = ArticleHelper.GetArticle(path, language);

        if (article is null)
        {
            await interaction.ReplyError(ErrorCodes.NotFound);
            return;
        }

        await interaction.Reply(article);
    }
}
