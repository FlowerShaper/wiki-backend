using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentVoteRoute : IAPIRoute
{
    public string Path => "/comments/:id/vote";
    public HttpMethod Method => HttpMethod.Post;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(ErrorCodes.CommentNotFound);
            return;
        }

        var vote = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

        if (!int.TryParse(vote, out var voteValue))
        {
            await interaction.ReplyError(ErrorCodes.InvalidParameter);
            return;
        }

        voteValue = Math.Clamp(voteValue, -1, 1);
        comment.SetVote(interaction.UserID, voteValue);

        CommentHelper.Update(comment);
        await interaction.Reply();
    }
}
