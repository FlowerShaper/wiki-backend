using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Models.Users;
using CamelliaWiki.Backend.Utils;
using Midori.API.Attributes;
using Midori.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/comments")]
public class CommentsController
{
    private readonly DatabaseContext database;

    public CommentsController(DatabaseContext database)
    {
        this.database = database;
    }

    [Authenticated]
    [HttpRoute("/:id", APIMethod.Patch)]
    public APIReturn<object> Edit(User auth, string id, [Source(ParameterSource.Body)] string content)
    {
        var comment = database.Comments.Find(id);
        if (comment is null) return Returns.NotFound();

        if (comment.AuthorID != auth.ID)
            return Returns.Message(HttpStatusCode.Forbidden, "You cannot edit other people's comments.");

        using (database.EditAndSave())
        {
            comment.Content = content.Sanitize();
            comment.LastEdited = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        return Returns.NoContent();
    }

    [Authenticated]
    [HttpRoute("/:id", APIMethod.Delete)]
    public APIReturn<object> Delete(User auth, string id)
    {
        var comment = database.Comments.Find(id);
        if (comment is null) return Returns.NotFound();

        if (comment.AuthorID != auth.ID && !auth.IsStaff)
            return Returns.Message(HttpStatusCode.Forbidden, "You cannot delete other people's comments.");

        using (database.EditAndSave())
            database.Comments.Remove(comment);

        return Returns.NoContent();
    }

    [Authenticated]
    [HttpRoute("/:id/reply", APIMethod.Post)]
    public APIReturn<object> Reply(User auth, string id, [Source(ParameterSource.Body)] string content)
    {
        var comment = database.Comments.Find(id);
        if (comment is null) return Returns.NotFound();

        if (comment.AuthorID != auth.ID)
            return Returns.Message(HttpStatusCode.Forbidden, "You cannot edit other people's comments.");

        using (database.EditAndSave())
            return comment.Reply(database, auth, content);
    }

    [Authenticated]
    [HttpRoute("/:id/vote", APIMethod.Post)]
    public APIReturn<object> Vote(User auth, string id, [Source(ParameterSource.Body)] string vote)
    {
        var comment = database.Comments.Find(id);
        if (comment is null) return Returns.NotFound();

        if (!int.TryParse(vote, out var value))
            return Returns.Message(HttpStatusCode.BadRequest, "Invalid vote value.");

        value = Math.Clamp(value, -1, 1);

        using (database.EditAndSave())
            comment.SetVote(auth.ID, value);

        return Returns.NoContent();
    }
}
