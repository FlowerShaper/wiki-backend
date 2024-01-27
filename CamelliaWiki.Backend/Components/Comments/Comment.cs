using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Components.Comments;

public class Comment
{
    [BsonId]
    public ObjectId ID { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Discord ID of the author of this comment.
    /// </summary>
    [BsonElement("author")]
    public ulong AuthorID { get; set; } = 0;

    /// <summary>
    /// The content of this comment.
    /// </summary>
    [BsonElement("content")]
    public string Content { get; set; } = "";

    /// <summary>
    /// The slug of the post this comment is on.
    /// </summary>
    [BsonElement("slug")]
    public string PostSlug { get; set; } = "";

    /// <summary>
    /// The timestamp of this comment.
    /// </summary>
    [BsonElement("time")]
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    /// <summary>
    /// The ID of the parent comment. Used for replies. Null if this is a top-level comment.
    /// </summary>
    [BsonElement("parent")]
    public ObjectId? ParentID { get; set; }
}
