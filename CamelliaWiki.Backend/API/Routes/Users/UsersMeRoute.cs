using System.Net;
using CamelliaWiki.Backend.API.Components;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Routes.Users;

public class UsersMeRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/users/@me";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction) => await interaction.Reply(HttpStatusCode.OK, interaction.User);
}
