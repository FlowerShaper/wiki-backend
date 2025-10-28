using CamelliaWiki.Backend.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Routes;

public class AssetRoute : IWikiAPIRoute
{
    public string RoutePath => "/cdn";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringQuery("path", out var path))
        {
            await interaction.ReplyMessage(HttpStatusCode.BadRequest, "Missing 'path' query.");
            return;
        }

        path = path.TrimStart('/');
        var fs = Path.Combine(Program.Config.DataDirectory, "_assets", path);

        if (!File.Exists(fs))
        {
            await interaction.ReplyMessage(HttpStatusCode.NotFound, "");
            return;
        }

        var data = await File.ReadAllBytesAsync(fs);
        await interaction.ReplyData(data, Path.GetFileName(fs));
    }
}
