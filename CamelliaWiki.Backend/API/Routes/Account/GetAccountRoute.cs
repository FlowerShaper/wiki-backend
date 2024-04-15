using CamelliaWiki.Backend.API.Components;

namespace CamelliaWiki.Backend.API.Routes.Account;

public class GetAccountRoute : IAPIRoute
{
    public string Path => "/account";
    public HttpMethod Method => HttpMethod.Get;
    public bool RequiresAuthentication => true;

    public async void Handle(APIInteraction interaction) => await interaction.Reply(interaction.User);
}
