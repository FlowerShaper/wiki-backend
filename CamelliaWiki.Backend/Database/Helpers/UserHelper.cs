using CamelliaWiki.Backend.Components.Users;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class UserHelper
{
    private static IMongoCollection<User> users => MongoDatabase.GetCollection<User>("users");

    public static User? Get(ulong id, bool allowFallback = true)
    {
        if (!allowFallback)
            return null;

        return users.Find(u => u.ID == id).FirstOrDefault();
    }
}
