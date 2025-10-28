using CamelliaWiki.Backend.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Error;

public class MethodNotAllowedRoute : IWikiAPIRoute
{
    public string RoutePath => "/405";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction) => await interaction.ReplyMessage(HttpStatusCode.MethodNotAllowed, "Method not allowed.");
}
