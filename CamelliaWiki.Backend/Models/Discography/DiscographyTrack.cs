using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CamelliaWiki.Backend.Database;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[JsonObject(MemberSerialization.OptIn)]
public class DiscographyTrack : IDiscographySearchable
{
    [Key, Column("id"), Required, MaxLength(256)]
    public string ID { get; set; } = string.Empty;

    [Column("title"), Required, MaxLength(256)]
    [JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [Column("title_romanized"), MaxLength(256)]
    [JsonProperty("title_romanized")]
    public string TitleRomanized { get; set; } = string.Empty;

    [Column("content")]
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [Column("length"), MaxLength(16)]
    [JsonProperty("length")]
    public string Length { get; set; } = string.Empty;

    [Column("bpm"), MaxLength(32)]
    [JsonProperty("bpm")]
    public string BPM { get; set; } = string.Empty;

    [Column("single")]
    [JsonProperty("single")]
    public bool Single { get; set; }

    [Column("release")]
    [JsonProperty("release")]
    public DiscographyRelease Release { get; set; } = null!;

    [Column("albums")]
    [JsonProperty("albums")]
    public ICollection<string> Albums { get; set; } = [];

    [Column("covers")]
    [JsonProperty("covers")]
    public ICollection<DiscographyCover> Covers { get; set; } = [];

    [Column("credits")]
    [JsonProperty("credits")]
    public ICollection<DiscographyCredit> Credits { get; set; } = [];

    [Column("links")]
    [JsonProperty("links")]
    public ICollection<DiscographyLink> Links { get; set; } = [];

    public object ToAPI(DatabaseContext database) => new
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
            var album = database.Albums.Find(x);

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
