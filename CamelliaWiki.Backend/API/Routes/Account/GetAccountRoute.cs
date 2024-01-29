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
            return new APIResponse { Code = ErrorCodes.NoAuthorizationHeader };

        if (!TokenCache.TryGet(auth, out var id))
            return new APIResponse { Code = ErrorCodes.InvalidToken };

        var user = UserHelper.Get(id, false);

        if (user == null)
            return new APIResponse { Code = ErrorCodes.UserNotFound };

        return new APIResponse { Data = user };
    }
}
