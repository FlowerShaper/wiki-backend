using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Models.Users;
using Midori.API.Attributes;
using Midori.API.Components;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/users")]
public class UsersController
{
    private readonly DatabaseContext database;

    public UsersController(DatabaseContext database)
    {
        this.database = database;
    }

    [Authenticated(Required = false)]
    [HttpRoute("/:id")]
    public APIReturn<User> Get(ulong id)
    {
        var user = database.Users.Find(id);
        return user is null ? Returns.NotFound() : user;
    }
}
