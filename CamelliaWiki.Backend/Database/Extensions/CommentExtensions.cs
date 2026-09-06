using CamelliaWiki.Backend.Models.Comments;
using CamelliaWiki.Backend.Models.Users;
using CamelliaWiki.Backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace CamelliaWiki.Backend.Database.Extensions;

public static class CommentExtensions
{
    public static Comment Create(this DbSet<Comment> set, string slug, User user, string content)
    {
        var c = new Comment
        {
            PostSlug = slug,
            AuthorID = user.ID,
            Content = content.Sanitize(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        c.SetVote(user.ID, 1);
        set.Add(c);
        return c;
    }

    public static Comment Reply(this Comment comment, DatabaseContext database, User user, string content)
    {
        var c = new Comment
        {
            PostSlug = comment.PostSlug,
            AuthorID = user.ID,
            Content = content.Sanitize(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ParentID = comment.ID
        };

        c.SetVote(user.ID, 1);
        database.Comments.Add(c);
        return comment;
    }
}
