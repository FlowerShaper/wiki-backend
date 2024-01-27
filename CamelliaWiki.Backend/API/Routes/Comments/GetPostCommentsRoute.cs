using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Comments;

public class GetPostCommentsRoute : IAPIRoute
{
    public string Path => "/posts/:slug/comments";
    public HttpMethod Method => HttpMethod.Get;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var slug = parameters["slug"];
        var comments = CommentHelper.GetPostComments(slug);
        return new APIResponse { Data = comments };
    }
}
