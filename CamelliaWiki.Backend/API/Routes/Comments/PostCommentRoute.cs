using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class PostCommentRoute : IAPIRoute
{
    public string Path => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Post;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var auth = context.Request.Headers["Authorization"];
        var slug = parameters["slug"];
        var content = new StreamReader(context.Request.InputStream).ReadToEnd();

        if (string.IsNullOrEmpty(auth))
        {
            return new APIResponse
            {
                Status = HttpStatusCode.Unauthorized,
                Message = "Missing authorization header!"
            };
        }

        if (string.IsNullOrEmpty(slug))
        {
            return new APIResponse
            {
                Status = HttpStatusCode.BadRequest,
                Message = "Missing slug!"
            };
        }

        if (string.IsNullOrEmpty(content))
        {
            return new APIResponse
            {
                Status = HttpStatusCode.BadRequest,
                Message = "Missing content!"
            };
        }

        if (!TokenCache.TryGet(auth, out var id))
        {
            return new APIResponse
            {
                Status = HttpStatusCode.Unauthorized,
                Message = "Invalid token!"
            };
        }

        var comment = CommentHelper.CreateComment(slug, id, content);
        return new APIResponse { Data = comment };
    }
}
