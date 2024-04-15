using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentRoute : IAPIRoute
{
    public string Path => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Post;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("slug", out var slug))
            return;

        var content = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
        {
            await interaction.ReplyError(ErrorCodes.MissingContent);
            return;
        }

        var comment = CommentHelper.CreateComment(slug, interaction.UserID, content);
        await interaction.Reply(comment);
    }
}
