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
    public ulong AuthorID { get; set; }

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
    public long LastEdited { get; set; }

    /// <summary>
    /// The votes on this comment.
    /// </summary>
    [JsonIgnore]
    [BsonElement("votes")]
    public Dictionary<string, long> Votes { get; set; } = new();

    /// <summary>
    /// The number of upvotes this comment has.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("ups")]
    public long UpVotes => Votes.Count(x => x.Value == 1);

    /// <summary>
    /// The number of downvotes this comment has.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("downs")]
    public long DownVotes => Votes.Count(x => x.Value == -1);

    /// <summary>
    /// The vote of the current user. 0 for no vote, 1 for upvote, -1 for downvote.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("vote")]
    public long YourVote { get; set; }

    /// <summary>
    /// The ID of the parent comment. Used for replies. Null if this is a top-level comment.
    /// </summary>
    [BsonElement("parent")]
    [JsonProperty("parent")]
    public ObjectId? ParentID { get; init; }

    /// <summary>
    /// The replies to this comment.
    /// </summary>
    [BsonIgnore]
    [JsonProperty("replies")]
    public List<Comment> Replies { get; set; } = new();

    public void SetVote(ulong uid, int voteValue)
    {
        if (!Votes.TryAdd(uid.ToString(), voteValue))
            Votes[uid.ToString()] = voteValue;
    }

    public void Populate(ulong uid)
    {
        Replies = CommentHelper.GetReplies(ID, uid).ToList();

        if (uid == 0)
            return;

        YourVote = Votes.GetValueOrDefault(uid.ToString(), 0);
    }
}
