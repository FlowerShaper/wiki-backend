using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Routes;

public class PostFeaturedArticleRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/featured";
    public HttpMethod Method => HttpMethod.Post;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.User.IsStaff)
        {
            await interaction.ReplyError(HttpStatusCode.Forbidden, "no.");
            return;
        }

        if (!interaction.TryParseBody<Payload>(out var payload))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "body is not valid json");
            return;
        }

        if (string.IsNullOrWhiteSpace(payload.Article))
        {
            await interaction.ReplyError(HttpStatusCode.BadRequest, "article cannot be empty");
            return;
        }

        DynamicsHelper.Set(DynamicDataType.FeaturedPost, payload.Article);
        await interaction.ReplyError(HttpStatusCode.OK, "ogke");
    }

    private class Payload
    {
        [JsonProperty("article")]
        public string Article { get; set; } = null!;
    }
}
