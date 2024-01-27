using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Components.Users;

/// <summary>
/// A cached user. Used in fallback scenarios where the user can't be found using the discord bot.
/// </summary>
public class CachedUser
{
    /// <summary>
    /// The user's ID.
    /// </summary>
    [BsonId]
    public ulong ID { get; set; }

    /// <summary>
    /// The user's username.
    /// </summary>
    [BsonElement("name")]
    public string Username { get; set; } = "";

    /// <summary>
    /// The url of the user's avatar.
    /// </summary>
    [BsonElement("avatar")]
    public string AvatarUrl { get; set; } = "";
}
