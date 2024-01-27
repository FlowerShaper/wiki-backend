using CamelliaWiki.Backend.API;
using CamelliaWiki.Backend.Database;

namespace CamelliaWiki.Backend;

public static class Program
{
    public static async Task Main()
    {
        MongoDatabase.Initialize("mongodb://localhost:27017", "camellia-wiki");

        var api = new APIServer();
        api.Start(1984);

        await Task.Delay(-1);
    }
}
