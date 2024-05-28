using System.Net;
using CamelliaWiki.Backend.API.Components;
using Midori.API.Components.Interfaces;

namespace CamelliaWiki.Backend.API.Routes.Account;

public class GetAccountRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/account";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction) => await interaction.Reply(HttpStatusCode.OK, interaction.User);
}
