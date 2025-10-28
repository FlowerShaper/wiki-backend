using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using Midori.API.Components.Interfaces;
using Midori.Networking;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Routes.Aliases;

public class AddAliasRoute : IWikiAPIRoute, INeedsAuthorization
{
    public string RoutePath => "/aliases";
    public HttpMethod Method => HttpMethod.Post;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.User.IsStaff)
        {
            await interaction.ReplyMessage(HttpStatusCode.Forbidden, "no.");
            return;
        }

        if (!interaction.TryParseBody<Payload>(out var payload))
        {
            await interaction.ReplyMessage(HttpStatusCode.BadRequest, "body is not valid json");
            return;
        }

        if (string.IsNullOrWhiteSpace(payload.Alias) || string.IsNullOrWhiteSpace(payload.Article))
        {
            await interaction.ReplyMessage(HttpStatusCode.BadRequest, "alias or article empty");
            return;
        }

        if (ArticleHelper.TryGetAlias(payload.Alias, out _))
        {
            await interaction.ReplyMessage(HttpStatusCode.BadRequest, "already exists");
            return;
        }

        ArticleHelper.AddAlias(payload.Alias, payload.Article);
        await interaction.ReplyMessage(HttpStatusCode.Created, "ogke");
    }

    private class Payload
    {
        [JsonProperty("alias")]
        public string Alias { get; set; } = null!;

        [JsonProperty("article")]
        public string Article { get; set; } = null!;
    }
}
