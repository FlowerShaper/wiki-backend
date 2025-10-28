using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Discography;

public class GetDiscographyRoute : IWikiAPIRoute
{
    public string RoutePath => "/discography";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        var albums = DiscographyHelper.AllAlbums;
        var tracks = DiscographyHelper.AllTracks;

        foreach (var album in albums)
            tracks.RemoveAll(t => !t.Single && album.Discs.Any(d => d.Tracks.Contains(t.ID)));

        await interaction.Reply(HttpStatusCode.OK, new
        {
            albums = albums.OrderBy(x => x.Release).Select(x => x.ToAPI()),
            tracks = tracks.OrderBy(x => x.Release).Select(x => x.ToAPI())
        });
    }
}
