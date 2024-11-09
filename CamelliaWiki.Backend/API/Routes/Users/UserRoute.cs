using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;

namespace CamelliaWiki.Backend.API.Routes.Users;

public class UserRoute : IWikiAPIRoute
{
    public string RoutePath => "/users/:id";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetULongParameter("id", out var id))
            return;

        var user = UserHelper.Get(id);

        if (user is null)
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "No user with the provided ID has been found.");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, user);
    }
}
