using CamelliaWiki.Backend.Components.Comments;
using CamelliaWiki.Backend.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class CommentHelper
{
    private static IMongoCollection<Comment> comments => MongoDatabase.GetCollection<Comment>("comments");

    public static List<Comment> All => comments.Find(_ => true).ToList();

    public static IEnumerable<Comment> GetPostComments(string slug, ulong uid)
    {
        var list = comments.Find(c => c.PostSlug == slug).ToList();
        list.ForEach(c => c.Populate(uid));
        return list;
    }

    public static Comment CreateComment(string slug, ulong id, string content)
    {
        var comment = new Comment
        {
            PostSlug = slug,
            AuthorID = id,
            Content = content.Sanitize(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        comments.InsertOne(comment);

        comment.SetVote(id, 1);
        Update(comment);
        return comment;
    }

    public static Comment CreateReply(this Comment parent, ulong uid, string content)
    {
        var comment = new Comment
        {
            PostSlug = parent.PostSlug,
            AuthorID = uid,
            Content = content.Sanitize(),
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ParentID = parent.ID
        };

        comments.InsertOne(comment);

        comment.SetVote(uid, 1);
        Update(comment);
        return comment;
    }

    public static void Update(Comment comment) => comments.ReplaceOne(c => c.ID == comment.ID, comment);

    public static void Delete(Comment comment)
    {
        comment.AuthorID = 0;
        comment.Content = "";
        Update(comment);
    }

    public static List<Comment> FromUser(ulong id) => comments.Find(c => c.AuthorID == id).ToList();

    public static bool TryGetComment(string id, out Comment o)
    {
        var objId = new ObjectId(id);

        o = comments.Find(c => c.ID == objId).FirstOrDefault();
        return o != null;
    }

    public static IEnumerable<Comment> GetReplies(ObjectId id, ulong uid)
    {
        var list = comments.Find(c => c.ParentID == id).ToList();
        list.ForEach(c => c.Populate(uid));
        return list;
    }
}
