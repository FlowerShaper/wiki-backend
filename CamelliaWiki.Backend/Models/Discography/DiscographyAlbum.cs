using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CamelliaWiki.Backend.Database;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyAlbum : IDiscographySearchable
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
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string Content { get; set; } = string.Empty;

    [Column("release")]
    [JsonProperty("release")]
    public DiscographyRelease Release { get; set; } = new();

    [Column("covers")]
    [JsonProperty("covers")]
    public ICollection<DiscographyCover> Covers { get; set; } = [];

    [Column("discs")]
    [JsonProperty("discs")]
    public ICollection<DiscographyDisc> Discs { get; set; } = [];

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
        release = Release,
        discs = Discs.Select(d => new
        {
            name = d.Name,
            tracks = d.Tracks.Select<string, object>(x =>
            {
                var track = database.Tracks.Find(x);

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
