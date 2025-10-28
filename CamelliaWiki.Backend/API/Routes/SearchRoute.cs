using CamelliaWiki.Backend.API.Components;
using CamelliaWiki.Backend.Database.Helpers;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Models.Discography;
using CamelliaWiki.Backend.Utils;
using Midori.Networking;
using Midori.Searching;

namespace CamelliaWiki.Backend.API.Routes;

public class SearchRoute : IWikiAPIRoute
{
    public string RoutePath => "/search";
    public HttpMethod Method => HttpMethod.Get;

    public async Task Handle(WikiAPIInteraction interaction)
    {
        if (!interaction.TryGetStringQuery("query", out var query))
        {
            await interaction.ReplyMessage(HttpStatusCode.BadRequest, "Please provide a search query.");
            return;
        }

        // searching in midori is current case-sensitive for some reason
        // really needs to be fixed someday
        query = query.ToLowerInvariant();

        LanguageUtils.TryParse(interaction.GetStringQuery("lang"), out var language);

        var articleFilter = new SearchFilter<Article>(query);
        var articles = articleFilter.Filter(ArticleHelper.GetAllUniqueArticles(language));

        var discFilter = new SearchFilter<IDiscographySearchable>(query);
        var discography = discFilter.Filter(DiscographyHelper.Searchable);

        foreach (var searchable in discography)
        {
            var track = searchable is DiscographyTrack;

            articles.Add(new Article
            {
                ID = $"/discography/{(track ? "tracks" : "albums")}/{searchable.ID}:en",
                Metadata = new ArticleMetadata
                {
                    Title = searchable.Title,
                    Description = track ? "Track" : "Album",
                    Image = searchable.ImageUrl ?? string.Empty
                }
            });
        }

        await interaction.Reply(HttpStatusCode.OK, articles);
    }
}
