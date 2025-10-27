using Midori.Searching;

namespace CamelliaWiki.Backend.Models.Discography;

public interface IDiscographySearchable
{
    string ID { get; set; }

    string Title { get; }
    string TitleRomanized { get; }

    string? ImageUrl { get; }

    [Searchable("title")]
    string SearchableTitle { get; }

    [Searchable("title_romanized")]
    string SearchableTitleRomanized { get; }
}
