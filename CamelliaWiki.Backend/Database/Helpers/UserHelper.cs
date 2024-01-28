using CamelliaWiki.Backend.Bot;
using CamelliaWiki.Backend.Components.Users;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class UserHelper
{
    private static IMongoCollection<User> users => MongoDatabase.GetCollection<User>("users");

    public static User? Get(ulong id, bool allowFallback = true)
    {
        var user = DiscordBot.GetUser(id);
        var cached = users.Find(u => u.ID == id).FirstOrDefault();

#pragma warning disable CS8604 // Possible null reference argument.
        if (user == null)
#pragma warning restore CS8604 // Possible null reference argument.
        {
            if (!allowFallback)
                return null;

            return cached;
        }

        var u = new User
        {
            ID = user.Id,
            Username = user.Username,
            AvatarUrl = user.AvatarUrl,
            Color = user.Color.ToString()
        };

        if (cached == null)
            users.InsertOne(u);
        else
            users.ReplaceOne(x => x.ID == id, u);

        return u;
    }
}
