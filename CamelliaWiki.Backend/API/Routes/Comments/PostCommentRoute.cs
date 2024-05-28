using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Post;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("slug", out var slug))
            return;

        var content = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "");
            return;
        }

        var comment = CommentHelper.CreateComment(slug, interaction.UserID, content);
        await interaction.Reply(HttpStatusCode.OK, comment);
    }
}
