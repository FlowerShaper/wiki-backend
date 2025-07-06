using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PatchCommentRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/comments/:id";
    public HttpMethod Method => HttpMethod.Patch;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var content = await new StreamReader(interaction.Request.BodyStream).ReadToEndAsync();

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

        if (comment.AuthorID != interaction.UserID)
        {
            await interaction.ReplyError(HttpStatusCode.Unauthorized, "");
            return;
        }

        comment.Content = content;
        comment.LastEdited = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        CommentHelper.Update(comment);
        await interaction.Reply(HttpStatusCode.OK);
    }
}
