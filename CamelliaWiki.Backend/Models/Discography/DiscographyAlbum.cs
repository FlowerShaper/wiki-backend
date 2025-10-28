using CamelliaWiki.Backend.Database.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyAlbum : IDiscographySearchable
{
    [BsonId]
    public string ID { get; set; } = string.Empty;

    [BsonElement("title")]
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("title_romanized")]
    [JsonProperty("title_romanized")]
    public string TitleRomanized { get; set; } = string.Empty;

    [BsonElement("content")]
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [BsonElement("release")]
    [JsonProperty("release")]
    public DiscographyRelease Release { get; set; } = new();

    [BsonElement("covers")]
    [JsonProperty("covers")]
    public DiscographyCover[] Covers { get; set; } = Array.Empty<DiscographyCover>();

    [BsonElement("discs")]
    [JsonProperty("discs")]
    public DiscographyDisc[] Discs { get; set; } = Array.Empty<DiscographyDisc>();

    [BsonElement("credits")]
    [JsonProperty("credits")]
    public DiscographyCredit[] Credits { get; set; } = Array.Empty<DiscographyCredit>();

    [BsonElement("links")]
    [JsonProperty("links")]
    public DiscographyLink[] Links { get; set; } = Array.Empty<DiscographyLink>();

    public object ToAPI() => new
    {
        id = ID,
        title = Title,
        title_romanized = TitleRomanized,
        content = Content,
        release = Release,
        discs = Discs.Select(d => new
        {
            name = d.Name,
            tracks = d.Tracks.Select<string, object>(x =>
            {
                var track = DiscographyHelper.GetTrack(x);

                if (track is not null)
                {
                    return new
                    {
                        id = track.ID,
                        title = track.Title,
                        title_romanized = track.TitleRomanized,
                        length = track.Length
                    };
                }

                return new { id = x };
            })
        }),
        covers = Covers,
        credits = Credits,
        links = Links
    };

    string? IDiscographySearchable.ImageUrl => Covers.FirstOrDefault()?.Url;
    string IDiscographySearchable.SearchableTitle => Title.ToLowerInvariant();
    string IDiscographySearchable.SearchableTitleRomanized => TitleRomanized.ToLowerInvariant();
}
