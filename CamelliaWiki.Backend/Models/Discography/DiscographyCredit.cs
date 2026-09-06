using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[Owned]
public class DiscographyCredit
{
    [Column("role")]
    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [Column("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
