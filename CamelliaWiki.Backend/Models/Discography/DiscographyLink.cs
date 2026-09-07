using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[Owned]
public class DiscographyLink
{
    [Column("label")]
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;

    [Column("url")]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
