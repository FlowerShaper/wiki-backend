using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;

namespace CamelliaWiki.Backend.API.Routes.Account;

public class GetAccountRoute : IAPIRoute
{
    public string Path => "/account";
    public HttpMethod Method => HttpMethod.Get;

    public APIResponse Handle(HttpListenerContext context, RequestParameters parameters)
    {
        var auth = context.Request.Headers["Authorization"];

        if (string.IsNullOrEmpty(auth))
        {
            return new APIResponse
            {
                Status = HttpStatusCode.Unauthorized,
                Message = "Missing authorization header!"
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

        var user = UserHelper.Get(id, false);

        if (user == null)
        {
            return new APIResponse
            {
                Status = HttpStatusCode.NotFound,
                Message = "User not found!"
            };
        }

        return new APIResponse { Data = user };
    }
}
