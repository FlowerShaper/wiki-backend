using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class DeleteCommentRoute : IAPIRoute
{
    public string Path => "/comments/:id";
    public HttpMethod Method => HttpMethod.Delete;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(ErrorCodes.NotFound);
            return;
        }

        if (comment.AuthorID != interaction.UserID && !UserHelper.IsStaff(interaction.UserID))
        {
            await interaction.ReplyError(ErrorCodes.NoPermission);
            return;
        }

        CommentHelper.Delete(comment);
        await interaction.Reply();
    }
}
