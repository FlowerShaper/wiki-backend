using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PatchCommentRoute : IAPIRoute
{
    public string Path => "/comments/:id";
    public HttpMethod Method => HttpMethod.Patch;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var content = new StreamReader(interaction.Request.InputStream).ReadToEnd();

        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
        {
            await interaction.ReplyError(ErrorCodes.MissingContent);
            return;
        }

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(ErrorCodes.CommentNotFound);
            return;
        }

        if (comment.AuthorID != interaction.UserID)
        {
            await interaction.ReplyError(ErrorCodes.NoPermission);
            return;
        }

        comment.Content = content;
        comment.LastEdited = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        CommentHelper.Update(comment);
        await interaction.Reply();
    }
}
