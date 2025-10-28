using CamelliaWiki.Backend.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Error;

public class NotFoundRoute : IWikiAPIRoute
{
    public string RoutePath => "/404";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction) => await interaction.ReplyMessage(HttpStatusCode.NotFound, "The requested route does not exist.");
}
