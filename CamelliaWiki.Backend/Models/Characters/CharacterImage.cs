using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Characters;

[Owned]
public class CharacterImage
{
    [Column("src")]
    [JsonProperty("src")]
    public string ImageSource { get; set; } = string.Empty;

    [Column("alt")]
    [JsonProperty("alt")]
    public string AltText { get; set; } = string.Empty;
}
