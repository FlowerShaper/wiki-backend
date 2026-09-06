using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamelliaWiki.Backend.Models.Articles;

[Table("alias")]
public class ArticleAlias
{
    [Key, Column("id"), Required, MaxLength(256)]
    public string Alias { get; init; } = string.Empty;

    [Column("article"), Required, MaxLength(256)]
    public string Article { get; init; } = string.Empty;
}
