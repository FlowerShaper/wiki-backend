using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Components.Users;
using MongoDB.Driver;
using DSharpPlus;
using DSharpPlus.Entities;

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
        catch
        {
            Logger.Log($"Failed to get user {id}");

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

        var u = new User
        {
            ID = user.Id,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl,
            Color = user.Color.ToString(),
            JoinDate = hasJoinDate ? cached!.JoinDate : DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            IsStaff = user.Permissions.HasPermission(Permissions.ModerateMembers)
        };

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
