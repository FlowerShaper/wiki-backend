using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CamelliaWiki.Backend.Models.Users;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Comments;

[Table("comment")]
public class Comment
{
    #region Stored

    [Key, Column("id"), Required, MaxLength(36)]
    [JsonProperty("id")]
    public string ID { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Discord ID of the author of this comment.
    /// </summary>
    [Column("author"), Required]
    [JsonIgnore]
    public ulong AuthorID { get; set; }

    /// <summary>
    /// The slug of the post this comment is on.
    /// </summary>
    [Column("slug"), Required, MaxLength(80)]
    [JsonProperty("slug")]
    public string PostSlug { get; init; } = "";

    /// <summary>
    /// The content of this comment.
    /// </summary>
    [Column("content"), Required, MaxLength(2048)]
    [JsonProperty("content")]
    public string Content { get; set; } = "";

    /// <summary>
    /// The timestamp of this comment.
    /// </summary>
    [Column("time"), Required]
    [JsonProperty("time")]
    public long Timestamp { get; init; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    /// <summary>
    /// The last time this comment was edited.
    /// </summary>
    [Column("edited")]
    [JsonProperty("edited")]
    public long LastEdited { get; set; }

    /// <summary>
    /// The ID of the parent comment. Used for replies. Null if this is a top-level comment.
    /// </summary>
    [Column("parent"), MaxLength(36)]
    [JsonProperty("parent")]
    public string? ParentID { get; init; }

    [ForeignKey(nameof(ParentID))]
    [JsonIgnore]
    public Comment? Parent { get; init; }

    [InverseProperty(nameof(CommentVote.Comment))]
    public ICollection<CommentVote> Votes { get; init; } = [];

    /*/// <summary>
    /// The votes on this comment.
    /// </summary>
    [JsonIgnore]
    [Column("votes")]
    public Dictionary<string, long> Votes { get; set; } = new();*/

    #endregion

    /// <summary>
    /// The author of this comment.
    /// </summary>
    [ForeignKey(nameof(AuthorID))]
    [JsonProperty("author")]
    public User? Author { get; init; }

    /*/// <summary>
    /// The number of upvotes this comment has.
    /// </summary>
    [NotMapped]
    [JsonProperty("ups")]
    public long UpVotes => Votes.Count(x => x.Value == 1);

    /// <summary>
    /// The number of downvotes this comment has.
    /// </summary>
    [NotMapped]
    [JsonProperty("downs")]
    public long DownVotes => Votes.Count(x => x.Value == -1);*/

    /// <summary>
    /// The vote of the current user. 0 for no vote, 1 for upvote, -1 for downvote.
    /// </summary>
    [NotMapped]
    [JsonProperty("vote")]
    public long YourVote { get; set; }

    /// <summary>
    /// The replies to this comment.
    /// </summary>
    [InverseProperty(nameof(Parent))]
    [JsonProperty("replies")]
    public List<Comment> Replies { get; set; } = new();

    public void SetVote(ulong uid, int voteValue)
    {
        /*if (!Votes.TryAdd(uid.ToString(), voteValue))
            Votes[uid.ToString()] = voteValue;*/
    }

    public void Populate(ulong uid)
    {
        if (uid == 0) return;

        /*YourVote = Votes.GetValueOrDefault(uid.ToString(), 0);*/
    }
}
