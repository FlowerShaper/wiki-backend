using DSharpPlus;
using DSharpPlus.Entities;

namespace CamelliaWiki.Backend.Bot;

public static class DiscordBot
{
    private const ulong guild_id = 435720333786480641;

    private static DiscordClient bot { get; set; } = null!;

    public static async Task StartAsync()
    {
        if (bot != null)
            throw new Exception("Bot is already running!");

        bot = new DiscordClient(new DiscordConfiguration
        {
            Token = Program.Config.Token,
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.All,
            AutoReconnect = true,
            MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.None
        });

        await bot.ConnectAsync();
    }

    public static async Task LoadUser(ulong id) => await bot.GetUserAsync(id, true);

    public static DiscordMember? GetUser(ulong id)
    {
        var guild = bot.Guilds[guild_id];
        return guild.GetMemberAsync(id).Result;
    }
}
