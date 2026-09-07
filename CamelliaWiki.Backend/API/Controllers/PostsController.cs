using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Database.Extensions;
using CamelliaWiki.Backend.Models.Comments;
using CamelliaWiki.Backend.Models.Users;
using Midori.API.Attributes;
using Midori.API.Components;
using Midori.Networking;

namespace CamelliaWiki.Backend.API.Controllers;

[Controller("/posts")]
public class PostsController
{
    private readonly DatabaseContext database;

    public PostsController(DatabaseContext database)
    {
        this.database = database;
    }

    [HttpRoute("/:slug/comments")]
    public APIReturn<Comment[]> ListComments(string slug)
        => database.Comments.Where(x => x.PostSlug == slug)
                   .OrderByDescending(x => x.Timestamp)
                   .ToArray();

    [Authenticated]
    [HttpRoute("/:slug/comments", APIMethod.Post)]
    public APIReturn<Comment> CreateComment(User auth, string slug, [Source(ParameterSource.Body)] string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return Returns.Message(HttpStatusCode.BadRequest, "Content cannot be empty.");

        using (database.EditAndSave())
            return database.Comments.Create(slug, auth, content);
    }
}
