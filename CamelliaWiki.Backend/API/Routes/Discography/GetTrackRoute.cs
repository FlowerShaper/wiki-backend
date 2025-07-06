using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes.Discography;

public class GetTrackRoute : IWikiAPIRoute
{
    public string RoutePath => "/discography/tracks/:id";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringParameter("id", out var id))
            return;

        var track = DiscographyHelper.GetTrack(id);

        if (track is null)
        {
            await interaction.ReplyError(HttpStatusCode.NotFound, "Could not find the specified track.");
            return;
        }

        await interaction.Reply(HttpStatusCode.OK, track.ToAPI());
    }
}
