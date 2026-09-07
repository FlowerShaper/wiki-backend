using CamelliaWiki.Backend.API.Payloads;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Models.Articles;
using Midori.API.Attributes;
using Midori.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/aliases")]
public class AliasesController
{
    private readonly DatabaseContext database;

    public AliasesController(DatabaseContext database)
    {
        this.database = database;
    }

    [Authenticated(Scopes.STAFF)]
    [HttpRoute("/", APIMethod.Post)]
    public APIReturn<object> Create([Source(ParameterSource.Body)] AliasCreatePayload payload)
    {
        var alias = database.Aliases.Find(payload.Alias);
        if (alias is not null) return Returns.Message(HttpStatusCode.BadRequest, "Alias already exists.");

        using (database.EditAndSave())
            database.Aliases.Add(new ArticleAlias { Alias = payload.Alias, Article = payload.Article });

        return Returns.Created();
    }
}
