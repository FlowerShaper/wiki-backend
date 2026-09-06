using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[Owned]
public class DiscographyCover
{
    [Column("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Column("url")]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
