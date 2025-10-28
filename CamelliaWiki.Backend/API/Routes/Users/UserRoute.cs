using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Users;

public class UserRoute : IWikiAPIRoute
{
    public string RoutePath => "/users/:i";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        // its currently just 'i' instead of "id" because midori doesn't
        // sort routes properly and will always prefer :id before @me
        // it's also length sorted
        if (!interaction.TryGetULongParameter("i", out var id))
            return;

        var user = UserHelper.Get(id);

        if (user is null)
        {
            await interaction.ReplyMessage(HttpStatusCode.NotFound, "No user with the provided ID has been found.");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, user);
    }
}
