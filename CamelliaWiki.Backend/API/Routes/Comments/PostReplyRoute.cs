using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostReplyRoute : IAPIRoute
{
    public string Path => "/comments/:id/reply";
    public HttpMethod Method => HttpMethod.Post;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var content = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

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

        var reply = comment.CreateReply(interaction.UserID, content);
        await interaction.Reply(reply);
    }
}
