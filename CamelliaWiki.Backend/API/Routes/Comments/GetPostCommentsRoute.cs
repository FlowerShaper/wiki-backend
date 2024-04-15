using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class GetPostCommentsRoute : IAPIRoute
{
    public string Path => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Get;
    public bool RequiresAuthentication => false;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("slug", out var slug))
            return;

        var comments = CommentHelper.GetPostComments(slug, interaction.UserID);
        await interaction.Reply(comments);
    }
}
