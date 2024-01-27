using System.Net;
using CamelliaWiki.Backend.API.Components;

namespace CamelliaWiki.Backend.API.Routes;

public class IndexRoute : IAPIRoute
{
    public string Path => "/";
    public HttpMethod Method => HttpMethod.Get;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        return new APIResponse { Message = "cool" };
    }
}
