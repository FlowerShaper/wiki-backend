using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Models.Users;
using DSharpPlus;
using DSharpPlus.Entities;
using Midori.Logging;

namespace CamelliaWiki.Backend.Database.Extensions;

public static class UserExtensions
{
    public static User? GetUser(this DatabaseContext database, ulong id, bool allowFallback = true)
    {
        if (id == 0)
            return null;

        DiscordMember? user;
        var cached = database.Users.Find(id);

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

        var u = cached ?? new User { ID = user.Id };

        using (database.EditAndSave())
        {
            u.Username = user.Username;
            u.Nickname = user.Nickname;
            u.AvatarUrl = user.AvatarUrl;
            u.BannerUrl = user.BannerUrl ?? cached?.BannerUrl ?? "";
            u.RoleColor = user.Color.ToString();
            u.JoinDate = hasJoinDate ? cached!.JoinDate : DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            u.IsStaff = user.Permissions.HasPermission(Permissions.ModerateMembers);
            if (cached == null) database.Users.Add(u);
        }

        return u;
    }
}
