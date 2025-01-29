using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostReplyRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/comments/:id/reply";
    public HttpMethod Method => HttpMethod.Post;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var content = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "");
            return;
        }

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        var reply = comment.CreateReply(interaction.UserID, content);
        await interaction.Reply(HttpStatusCode.OK, reply);
    }
}
