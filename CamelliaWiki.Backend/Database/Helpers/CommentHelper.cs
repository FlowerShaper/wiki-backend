namespace CamelliaWiki.Backend.Database.Helpers;

public static class CommentHelper
{
    /*public static List<Comment> FromUser(ulong id) => comments.Find(c => c.AuthorID == id).ToList();

    public static IEnumerable<Comment> GetReplies(ObjectId id, ulong uid)
    {
        var list = comments.Find(c => c.ParentID == id).ToList();
        list.ForEach(c => c.Populate(uid));
        return list;
    }*/
}
