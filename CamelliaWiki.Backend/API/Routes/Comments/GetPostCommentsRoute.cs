using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class GetPostCommentsRoute : IAPIRoute
{
    public string Path => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Get;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var slug = parameters["slug"];
        var auth = context.Request.Headers["Authorization"];

        ulong uid = 0;

        if (!string.IsNullOrEmpty(auth))
            TokenCache.TryGet(auth, out uid);

        var comments = CommentHelper.GetPostComments(slug, uid);

        return new APIResponse { Data = comments };
    }
}
