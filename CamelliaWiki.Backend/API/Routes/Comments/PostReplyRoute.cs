using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostReplyRoute : IAPIRoute
{
    public string Path => "/comments/:id/reply";
    public HttpMethod Method => HttpMethod.Post;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var auth = context.Request.Headers["Authorization"];
        var id = parameters["id"];
        var content = new StreamReader(context.Request.InputStream).ReadToEnd();

        if (string.IsNullOrEmpty(auth))
            return new APIResponse { Code = ErrorCodes.NoAuthorizationHeader };

        if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            return new APIResponse { Code = ErrorCodes.MissingContent };

        if (!TokenCache.TryGet(auth, out var uid))
            return new APIResponse { Code = ErrorCodes.InvalidToken };

        if (!CommentHelper.TryGetComment(id, out var comment))
            return new APIResponse { Code = ErrorCodes.CommentNotFound };

        var reply = comment.CreateReply(uid, content);
        return new APIResponse { Data = reply };
    }
}
