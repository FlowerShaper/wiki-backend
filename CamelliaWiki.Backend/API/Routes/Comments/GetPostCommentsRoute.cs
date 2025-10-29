using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class GetPostCommentsRoute : IWikiAPIRoute
{
    public string RoutePath => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("slug", out var slug))
            return;

        var comments = CommentHelper.GetPostComments(slug, interaction.UserID)
                                    .OrderByDescending(x => x.Timestamp).ToList();

        await interaction.Reply(HttpStatusCode.OK, comments);
    }
}
