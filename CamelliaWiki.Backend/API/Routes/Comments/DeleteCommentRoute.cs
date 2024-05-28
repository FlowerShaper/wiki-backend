using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class DeleteCommentRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/comments/:id";
    public HttpMethod Method => HttpMethod.Delete;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        if (comment.AuthorID != interaction.UserID && !UserHelper.IsStaff(interaction.UserID))
        {
            await interaction.ReplyError(HttpStatusCode.Unauthorized, "");
            return;
        }

        CommentHelper.Delete(comment);
        await interaction.Reply(HttpStatusCode.OK);
    }
}
