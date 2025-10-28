using System.Diagnostics;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Components.Views;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models;
using Midori.Logging;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes;

public class GetPopularArticleRoute : IWikiAPIRoute
{
    public string RoutePath => "/popular";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var views = Program.ViewManager.GetToday();

        Logger.Log($"fetched views at {stopwatch.ElapsedMilliseconds}ms", LoggingTarget.Network, LogLevel.Debug);

        var unique = new List<ArticleView>();

        foreach (var view in views)
        {
            if (unique.Any(x => x.IP == view.IP && x.Article == view.Article))
                continue;

            unique.Add(view);
        }

        Logger.Log($"sorted unique at {stopwatch.ElapsedMilliseconds}ms", LoggingTarget.Network, LogLevel.Debug);

        var highest = unique.GroupBy(x => x.Article).MaxBy(x => x.Count());

        Logger.Log($"found highest at {stopwatch.ElapsedMilliseconds}ms", LoggingTarget.Network, LogLevel.Debug);

        if (highest is null)
        {
            await interaction.ReplyMessage(HttpStatusCode.NotFound, "");
            return;
        }

        var article = ArticleHelper.GetArticle(highest.Key, Language.en);

        if (article is null)
        {
            await interaction.ReplyMessage(HttpStatusCode.NotFound, "");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, article);
        Logger.Log($"finished in {stopwatch.ElapsedMilliseconds}ms", LoggingTarget.Network, LogLevel.Debug);
        stopwatch.Stop();
    }
}
