using System.Reflection;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Components;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Markdown;
using Midori.API;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public static class Program
{
    public static Config Config { get; private set; } = null!;

    public static ViewManager ViewManager { get; private set; } = null!;
    public static VisitorManager Visitors { get; private set; } = null!;

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
        ViewManager = new ViewManager();
        Visitors = new VisitorManager();

        var api = new APIServer<WikiAPIInteraction>();
        api.AddRoutesFromAssembly<IWikiAPIRoute>(Assembly.GetExecutingAssembly());
        api.Start(1984);

        await Task.Delay(-1);
    }
}
