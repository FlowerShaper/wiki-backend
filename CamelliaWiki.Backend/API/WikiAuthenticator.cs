using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Utils;
using Midori.API;
using Midori.Networking;

namespace CamelliaWiki.Backend.API;

public class WikiAuthenticator : IAPIAuthenticator
{
    private readonly DatabaseContext database;

    public WikiAuthenticator(DatabaseContext database)
    {
        this.database = database;
    }

    public bool Authenticate(HttpServerContext ctx, out List<string> scopes, out Dictionary<string, object> data)
    {
        scopes = [];
        data = new Dictionary<string, object>();

        var token = ctx.Request.Headers["Authorization"];
        token ??= ctx.Request.Cookies["token"];

        if (string.IsNullOrEmpty(token))
            return false;

        token = token.Split(" ").Last().Trim();

        if (!TokenCache.TryGet(token, out var id))
            return false;

        var user = database.GetUser(id);
        if (user == null) return false;

        if (user.IsStaff)
            scopes.Add(Scopes.STAFF);

        data["auth"] = user;
        return true;
    }
}
