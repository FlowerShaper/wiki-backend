using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CamelliaWiki.Backend.Models.Comments;

[PrimaryKey(nameof(CommentID), nameof(UserID))]
[Table("comment-vote")]
public class CommentVote
{
    [Column("id"), Required, MaxLength(36)]
    public string CommentID { get; init; } = string.Empty;

    [Column("user"), Required]
    public ulong UserID { get; init; }

    [Column("value")]
    public bool? Value { get; set; }

    [ForeignKey(nameof(CommentID)), DeleteBehavior(DeleteBehavior.Cascade)]
    public Comment? Comment { get; init; }
}
