using System.Diagnostics.CodeAnalysis;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models.Users;
using CamelliaWiki.Backend.Utils;
using Midori.API;
using Midori.API.Components.Interfaces;
using Midori.API.Components.Json;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Components;

public class WikiAPIInteraction : JsonInteraction, IHasAuthorizationInfo
{
    protected override string[] AllowedHeaders => base.AllowedHeaders.Concat(extra_headers).ToArray();
    private static readonly string[] extra_headers = { "baggage", "sentry-trace" };

    protected override bool RespondOnInvalidParameter => false;

    public bool IsAuthorized => UserID != 0;
    public string AuthorizationError { get; private set; } = null!;

    public ulong UserID { get; private set; }
    public User User { get; private set; } = null!;

    protected override void OnPopulate()
    {
        var token = Request.Headers["Authorization"];
        // token ??= Request.Cookies["token"]?.Value;

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

    public bool TryParseBody<T>([NotNullWhen(true)] out T? result)
    {
        result = default!;

        if (Request.BodyStream == Stream.Null)
            return false;

        var body = new StreamReader(Request.BodyStream).ReadToEnd();

        try
        {
            result = body.Deserialize<T>();
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    public bool TryGetULongParameter(string name, out ulong value)
    {
        if (TryGetStringParameter(name, out var s) && ulong.TryParse(s, out value))
            return true;

        if (RespondOnInvalidParameter)
            ReplyMessage(HttpStatusCode.BadRequest, DefaultResponseStrings.InvalidParameter(name, "ulong")).Wait();

        value = 0L;
        return false;
    }
}
