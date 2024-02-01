using CamelliaWiki.Backend.Components.Users;
using CamelliaWiki.Backend.Database.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Components.Comments;

public class Comment
{
    [BsonId]
    [JsonProperty("id")]
    public ObjectId ID { get; init; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Discord ID of the author of this comment.
    /// </summary>
    [BsonElement("author")]
    [JsonIgnore]
    public ulong AuthorID { get; init; }

    /// <summary>
    /// The author of this comment.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("author")]
    public User Author => UserHelper.Get(AuthorID)!;

    /// <summary>
    /// The content of this comment.
    /// </summary>
    [BsonElement("content")]
    [JsonProperty("content")]
    public string Content { get; set; } = "";

    /// <summary>
    /// The slug of the post this comment is on.
    /// </summary>
    [BsonElement("slug")]
    [JsonProperty("slug")]
    public string PostSlug { get; init; } = "";

    /// <summary>
    /// The timestamp of this comment.
    /// </summary>
    [BsonElement("time")]
    [JsonProperty("time")]
    public long Timestamp { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    /// <summary>
    /// The last time this comment was edited.
    /// </summary>
    [BsonElement("edited")]
    [JsonProperty("edited")]
    public long LastEdited { get; set; } = 0;

    /// <summary>
    /// The ID of the parent comment. Used for replies. Null if this is a top-level comment.
    /// </summary>
    [BsonElement("parent")]
    [JsonProperty("parent")]
    public ObjectId? ParentID { get; init; }
}
