using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PatchCommentRoute : IAPIRoute
{
    public string Path => "/comments/:id";
    public HttpMethod Method => HttpMethod.Patch;

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

        if (comment.AuthorID != uid)
            return new APIResponse { Code = ErrorCodes.NoPermission };

        comment.Content = content;
        comment.LastEdited = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        CommentHelper.Update(comment);

        return new APIResponse { Data = comment };
    }
}
