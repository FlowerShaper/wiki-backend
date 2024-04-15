using CamelliaWiki.Backend.API;
using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Markdown;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public static class Program
{
    public static Config Config { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        if (!File.Exists("config.json"))
            throw new Exception("Config file not found!");

        Config = JsonConvert.DeserializeObject<Config>(await File.ReadAllTextAsync("config.json"))!;

        MongoDatabase.Initialize(Config.MongoStr, "camellia-wiki");

        if (args.Contains("--md"))
        {
            MarkdownProcessor.Run(Config.DataDirectory);
            return;
        }

        await DiscordBot.StartAsync();

        var api = new APIServer();
        api.Start(1984);

        await Task.Delay(-1);
    }
}
