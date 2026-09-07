using CamelliaWiki.Backend.Database;
using Midori.API.Attributes;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/discography")]
public class DiscographyController
{
    private readonly DatabaseContext database;

    public DiscographyController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/")]
    public APIReturn<object> List()
    {
        var tracks = database.Tracks.ToList();
        var albums = database.Albums.ToArray();

        foreach (var album in albums)
            tracks.RemoveAll(t => !t.Single && album.Discs.Any(d => d.Tracks.Contains(t.ID)));

        return new
        {
            albums = albums.OrderBy(x => x.Release).Select(x => x.ToAPI(database)),
            tracks = tracks.OrderBy(x => x.Release).Select(x => x.ToAPI(database))
        };
    }

    [HttpRoute("/albums/:id")]
    public APIReturn<object> Album(string id)
    {
        var album = database.Albums.Find(id);
        if (album is null) return Returns.NotFound("album");

        return album.ToAPI(database);
    }

    [HttpRoute("/tracks/:id")]
    public APIReturn<object> Track(string id)
    {
        var album = database.Tracks.Find(id);
        if (album is null) return Returns.NotFound("track");

        return album.ToAPI(database);
    }
}
