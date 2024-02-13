using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentVoteRoute : IAPIRoute
{
    public string Path => "/comments/:id/vote";
    public HttpMethod Method => HttpMethod.Post;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var auth = context.Request.Headers["Authorization"];
        var id = parameters["id"];

        if (string.IsNullOrEmpty(auth))
            return new APIResponse { Code = ErrorCodes.NoAuthorizationHeader };

        if (!TokenCache.TryGet(auth, out var uid))
            return new APIResponse { Code = ErrorCodes.InvalidToken };

        if (!CommentHelper.TryGetComment(id, out var comment))
            return new APIResponse { Code = ErrorCodes.CommentNotFound };

        var vote = new StreamReader(context.Request.InputStream).ReadToEnd();

        if (!int.TryParse(vote, out var voteValue))
            return new APIResponse { Code = ErrorCodes.InvalidParameter };

        voteValue = Math.Clamp(voteValue, -1, 1);
        comment.SetVote(uid, voteValue);

        CommentHelper.Update(comment);
        return new APIResponse();
    }
}
