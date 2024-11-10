using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Models.Users;
using MongoDB.Driver;
using DSharpPlus;
using DSharpPlus.Entities;
using Midori.Logging;
using Midori.Utils;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class UserHelper
{
    private static IMongoCollection<User> users => MongoDatabase.GetCollection<User>("users");

    public static User? Get(ulong id, bool allowFallback = true)
    {
        if (id == 0)
            return null;

        DiscordMember? user;
        var cached = users.Find(u => u.ID == id).FirstOrDefault();

        try
        {
            user = DiscordBot.GetUser(id);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, $"Failed to get user {id}", LoggingTarget.Network);

            if (!allowFallback)
                return null;

            return cached;
        }

#pragma warning disable CS8604 // Possible null reference argument.
        if (user == null)
#pragma warning restore CS8604 // Possible null reference argument.
        {
            if (!allowFallback)
                return null;

            return cached;
        }

        var hasJoinDate = cached != null && cached.JoinDate != 0;

        // ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        var u = new User
        {
            ID = user.Id,
            Username = user.Username,
            Nickname = user.Nickname ?? "",
            AvatarUrl = user.AvatarUrl,
            BannerUrl = user.BannerUrl ?? cached?.BannerUrl ?? "",
            Color = user.Color.ToString(),
            JoinDate = hasJoinDate ? cached!.JoinDate : DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsStaff = user.Permissions.HasPermission(Permissions.ModerateMembers)
        };
        // ReSharper enable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

        Logger.Log($"wah {user.BannerHash}");
        Logger.Log(u.Serialize());

        if (cached == null)
            users.InsertOne(u);
        else
            users.ReplaceOne(x => x.ID == id, u);

        return u;
    }

    public static bool IsStaff(ulong uid)
    {
        var user = Get(uid);
        return user is { IsStaff: true };
    }
}
