using System.Net;
using CamelliaWiki.Backend.API;
using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Midori.API;
using Midori.API.Handlers;
using Midori.Logging;
using Midori.Networking;
using Midori.Networking.Handlers;
using Midori.Utils.Extensions;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public static class Program
{
    public static Config Config { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        Logger.SaveToFiles = false;

        if (!File.Exists("config.json"))
        {
            Logger.Log("!!! CONFIG FILE NOT FOUND !!!", LoggingTarget.General, LogLevel.Warning);
            Config = new Config();
        }
        else
            Config = JsonConvert.DeserializeObject<Config>(await File.ReadAllTextAsync("config.json"))!;

        var builder = new HostApplicationBuilder();

        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new MidoriLoggerProvider());

        // DATABASE SETUP //
        builder.Services.AddDbContext<DatabaseContext>(c =>
        {
            // c.UseMongoDB(Config.MongoStr, "camellia-wiki");
            c.UseNpgsql(Config.PostgresConnection);

            if (!builder.Environment.IsDevelopment())
            {
                c.UseLoggerFactory(new NullLoggerFactory());
                return;
            }

            // c.EnableSensitiveDataLogging();
            c.EnableDetailedErrors();
        });

        // API SETUP //
        builder.Services.AddSingleton<IHttpReplyHandler, DefaultAPIReplyHandler>();
        builder.Services.AddScoped<IAPIAuthenticator, WikiAuthenticator>();
        builder.Services.AddHttpServer(c =>
        {
            c.Address = IPAddress.Loopback;
            c.Port = 1984;
        });

        var host = builder.Build();

        using (var scope = host.Services.CreateScope())
        {
            if (builder.Environment.IsProduction())
            {
                // this should be fine to do at runtime since there
                // are no multiple instances accessing the db at once
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await db.Database.MigrateAsync();
            }

            if (args.Contains("--md"))
            {
                var process = ActivatorUtilities.CreateInstance<DataProcessor>(scope.ServiceProvider);
                process.Start(Config.DataDirectory);
                return;
            }
        }

        var router = host.Services.GetRequiredService<HttpRouter>();
        router.AddMiddleware<WikiAtMeMiddleware>();
        router.RegisterControllersFromAssembly(typeof(Program).Assembly);

        await DiscordBot.StartAsync();
        await host.RunAsync();
    }
}
