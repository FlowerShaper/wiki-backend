using CamelliaWiki.Backend.Database.Helpers;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[JsonObject(MemberSerialization.OptIn)]
public class DiscographyTrack : IDiscographySearchable
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

    [BsonElement("length")]
    [JsonProperty("length")]
    public string Length { get; set; } = string.Empty;

    [BsonElement("bpm")]
    [JsonProperty("bpm")]
    public string BPM { get; set; } = string.Empty;

    [BsonElement("release")]
    [JsonProperty("release")]
    public DiscographyRelease Release { get; set; } = null!;

    [BsonElement("albums")]
    [JsonProperty("albums")]
    public string[] Albums { get; set; } = Array.Empty<string>();

    [BsonElement("covers")]
    [JsonProperty("covers")]
    public DiscographyCover[] Covers { get; set; } = Array.Empty<DiscographyCover>();

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
        length = Length,
        bpm = BPM,
        release = Release,
        albums = Albums.Select<string, object>(x =>
        {
            var album = DiscographyHelper.GetAlbum(x);

            if (album is not null)
            {
                return new
                {
                    id = album.ID,
                    title = album.Title,
                    title_romanized = album.TitleRomanized
                };
            }

            return new { id = x };
        }),
        covers = Covers,
        credits = Credits,
        links = Links
    };

    string? IDiscographySearchable.ImageUrl => Covers.FirstOrDefault()?.Url;
    string IDiscographySearchable.SearchableTitle => Title.ToLowerInvariant();
    string IDiscographySearchable.SearchableTitleRomanized => TitleRomanized.ToLowerInvariant();
}
