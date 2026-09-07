using CamelliaWiki.Backend.Database;
using Midori.API.Attributes;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/characters")]
public class CharactersController
{
    private readonly DatabaseContext database;

    public CharactersController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/")]
    public APIReturn<object[]> List() => database.Characters.ToList().Select(x => x.ToAPI()).ToArray();

    [HttpRoute("/:id")]
    public APIReturn<object> Specific(string id)
    {
        var character = database.Characters.Find(id);
        if (character is null) return Returns.NotFound("character");

        return character.ToAPI();
    }
}
