using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentVoteRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/comments/:id/vote";
    public HttpMethod Method => HttpMethod.Post;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        if (!CommentHelper.TryGetComment(id, out var comment))
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "");
            return;
        }

        var vote = await new StreamReader(interaction.Request.InputStream).ReadToEndAsync();

        if (!int.TryParse(vote, out var voteValue))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "");
            return;
        }

        voteValue = Math.Clamp(voteValue, -1, 1);
        comment.SetVote(interaction.UserID, voteValue);

        CommentHelper.Update(comment);
        await interaction.Reply(HttpStatusCode.OK);
    }
}
