using CamelliaWiki.Backend.Database.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Components.Users;

/// <summary>
/// A user. Database-stored users are only used in fall-back scenarios where we can't get the user from Discord.
/// </summary>
public class User
{
    /// <summary>
    /// The user's ID.
    /// </summary>
    [BsonId]
    [JsonProperty("id")]
    public ulong ID { get; set; }

    /// <summary>
    /// The user's username.
    /// </summary>
    [BsonElement("name")]
    [JsonProperty("name")]
    public string Username { get; set; } = "";

    /// <summary>
    /// The url of the user's avatar.
    /// </summary>
    [BsonElement("avatar")]
    [JsonProperty("avatar")]
    public string AvatarUrl { get; set; } = "";

    /// <summary>
    /// The user's role color.
    /// </summary>
    [BsonElement("color")]
    [JsonProperty("color")]
    public string Color { get; set; } = "";

    /// <summary>
    /// Join date of the user.
    /// </summary>
    [BsonElement("join")]
    [JsonProperty("join")]
    public long JoinDate { get; set; } = 0;

    /// <summary>
    /// Amount of comments the user has made.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("comments")]
    public int CommentCount => CommentHelper.FromUser(ID).Count;
}
