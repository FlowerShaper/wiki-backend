using CamelliaWiki.Backend.Components.Comments;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class CommentHelper
{
    private static IMongoCollection<Comment> comments => MongoDatabase.GetCollection<Comment>("comments");

    public static IEnumerable<Comment> GetPostComments(string slug) => comments.Find(c => c.PostSlug == slug).ToEnumerable();

    public static Comment CreateComment(string slug, ulong id, string content)
    {
        var comment = new Comment
        {
            PostSlug = slug,
            AuthorID = id,
            Content = content,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        comments.InsertOne(comment);
        return comment;
    }

    public static List<Comment> FromUser(ulong id) => comments.Find(c => c.AuthorID == id).ToList();
}
