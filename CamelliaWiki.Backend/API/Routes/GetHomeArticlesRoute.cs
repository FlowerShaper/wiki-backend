using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes;

public class GetHomeArticlesRoute : IWikiAPIRoute
{
    public string RoutePath => "/home";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        var list = new List<IEnumerable<Article>>();
        var value = DynamicsHelper.Get(DynamicDataType.HomePosts);

        if (string.IsNullOrWhiteSpace(value))
        {
            await interaction.Reply(HttpStatusCode.NotFound, list);
            return;
        }

        var categories = value.Split("::");

        foreach (var category in categories)
        {
            var split = category.Split(';');
            var articles = split.Select(s => ArticleHelper.GetArticle(s, Language.en));
            list.Add(articles.OfType<Article>());
        }

        await interaction.Reply(HttpStatusCode.OK, list);
    }
}
