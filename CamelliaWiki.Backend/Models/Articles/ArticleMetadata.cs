using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Articles;

[Table("article-meta")]
[PrimaryKey(nameof(ID), nameof(Language))]
[JsonObject(MemberSerialization.OptIn)]
public class ArticleMetadata
{
    [Key, Column("id"), Required, MaxLength(256)]
    public string ID { get; init; } = string.Empty;

    [Key, Column("lang"), Required]
    public ArticleLanguage Language { get; init; } = ArticleLanguage.en;

    [Column("title"), JsonProperty("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description"), JsonProperty("description")]
    public string Description { get; set; } = string.Empty;

    [Column("image"), JsonProperty("image")]
    public string Image { get; set; } = string.Empty;

    [Column("layout"), JsonProperty("layout")]
    public string Layout { get; set; } = string.Empty;

    [Column("type"), JsonProperty("type")]
    public ArticleType Type { get; set; } = ArticleType.Article;

    [Column("date"), JsonProperty("date")]
    public long? Date { get; set; }

    [ForeignKey($"{nameof(ID)},{nameof(Language)}"), DeleteBehavior(DeleteBehavior.Cascade)]
    public Article Article { get; init; } = null!;
}

public enum ArticleType
{
    /// <summary>
    /// Your typical wiki article.
    /// </summary>
    Article = 1,

    /// <summary>
    /// News about wiki updates or similar.
    /// </summary>
    News = 2
}
