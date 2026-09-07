using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Midori.Searching;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Articles;

[Table("article")]
[PrimaryKey(nameof(ID), nameof(Language))]
[JsonObject(MemberSerialization.OptIn)]
public class Article : IComparable<Article>
{
    #region Stored

    [Key, Column("path"), Required, MaxLength(256)]
    [Searchable("path"), JsonProperty("url")]
    public string ID { get; init; } = string.Empty;

    [Key, Column("lang"), Required]
    public ArticleLanguage Language { get; init; } = ArticleLanguage.en;

    [InverseProperty(nameof(ArticleMetadata.Article)), JsonProperty("meta")]
    public ArticleMetadata? Metadata { get; set; }

    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    [Column("content"), JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    #endregion

    #region Searching

    [NotMapped, JsonProperty("lang")]
    public string LanguageString => Language.ToString();

    [NotMapped, Searchable("title")]
    [Obsolete("Use Metadata.Title instead.")]
    public string SearchableTitle => Metadata?.Title ?? "";

    [NotMapped, Searchable("description")]
    [Obsolete("Use Metadata.Description instead.")]
    public string SearchableDescription => Metadata?.Description ?? "";

    #endregion

    public int CompareTo(Article? other)
    {
        if (ReferenceEquals(this, other))
            return 0;

        if (ReferenceEquals(null, other))
            return 1;

        return Metadata?.Date?.CompareTo(other.Metadata?.Date ?? 0) ?? 0;
    }
}
