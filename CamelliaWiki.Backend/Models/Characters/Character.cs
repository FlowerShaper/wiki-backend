using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Characters;

public class Character
{
    [Key, Column("id"), Required, MaxLength(128)]
    public string ID { get; set; } = string.Empty;

    [Column("name"), Required, MaxLength(128)]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Column("content")]
    [JsonProperty("content")]
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public string Content { get; set; } = string.Empty;

    [Column("images")]
    [JsonProperty("images")]
    public ICollection<CharacterImage> Images { get; set; } = [];

    public object ToAPI() => new
    {
        id = ID,
        name = Name,
        content = Content,
        images = Images
    };
}
