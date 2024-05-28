using CamelliaWiki.Backend.Components.Users;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Utils;
using Midori.API.Components;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Components;

public class WikiAPIInteraction : APIInteraction, IHasAuthorizationInfo
{
    public bool IsAuthorized => UserID != 0;
    public string AuthorizationError { get; private set; } = null!;

    public ulong UserID { get; private set; }
    public User User { get; private set; } = null!;

    protected override void OnPopulate()
    {
        var token = Request.Headers["Authorization"];
        token ??= Request.Cookies["token"]?.Value;

        if (string.IsNullOrEmpty(token))
        {
            AuthorizationError = "Missing token";
            return;
        }

        if (!TokenCache.TryGet(token, out var id))
        {
            AuthorizationError = "Invalid token";
            return;
        }

        var user = UserHelper.Get(id, false);

        if (user == null)
        {
            AuthorizationError = "User not found";
            return;
        }

        UserID = user.ID;
        User = user;
    }
}
