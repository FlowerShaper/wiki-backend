using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Discography;

public class GetAlbumRoute : IWikiAPIRoute
{
    public string RoutePath => "/discography/albums/:id";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var album = DiscographyHelper.GetAlbum(id);

        if (album is null)
        {
            await interaction.ReplyMessage(HttpStatusCode.NotFound, "Could not find the specified album.");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, album.ToAPI());
    }
}
