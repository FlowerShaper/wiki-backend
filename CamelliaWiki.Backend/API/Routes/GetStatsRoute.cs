using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes;

public class GetStatsRoute : IAPIRoute
{
    public string Path => "/stats";
    public HttpMethod Method => HttpMethod.Get;
    public bool RequiresAuthentication => false;

    public async void Handle(APIInteraction interaction)
    {
        var dict = new Dictionary<string, long>();

        foreach (var article in ArticleHelper.All)
        {
            var path = article.Path;

            if (!dict.TryAdd(path, 1))
                dict[path]++;
        }

        await interaction.Reply(new Statistics
        {
            Articles = dict.Count,
            Comments = CommentHelper.All.Count,
            UniqueVisitors = 0
        });
    }
}
