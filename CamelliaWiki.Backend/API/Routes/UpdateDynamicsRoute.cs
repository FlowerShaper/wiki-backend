using System.Net;
using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models;
using Midori.API.Components.Interfaces;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Routes;

public class UpdateDynamicsRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/dynamics";
    public HttpMethod Method => HttpMethod.Patch;

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

        var updatedAnything = false;

        if (!string.IsNullOrWhiteSpace(payload.FeaturedArticle))
        {
            DynamicsHelper.Set(DynamicDataType.FeaturedPost, payload.FeaturedArticle);
            updatedAnything = true;
        }

        if (!string.IsNullOrWhiteSpace(payload.HomeArticles))
        {
            DynamicsHelper.Set(DynamicDataType.HomePosts, payload.HomeArticles);
            updatedAnything = true;
        }

        if (updatedAnything)
            await interaction.ReplyError(HttpStatusCode.OK, "okge");
        else
            await interaction.ReplyError(HttpStatusCode.NotModified, "nothing changed.");
    }

    private class Payload
    {
        [JsonProperty("featured")]
        public string FeaturedArticle { get; set; } = null!;

        [JsonProperty("home")]
        public string HomeArticles { get; set; } = null!;
    }
}
