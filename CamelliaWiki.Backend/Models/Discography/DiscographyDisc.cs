using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[Owned]
public class DiscographyDisc
{
    [Column("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Column("tracks")]
    [JsonProperty("tracks")]
    public ICollection<string> Tracks { get; set; } = [];
}
