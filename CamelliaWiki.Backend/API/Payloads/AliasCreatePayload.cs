using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Payloads;

public class AliasCreatePayload
{
    [JsonProperty("alias"), Required]
    public string Alias { get; set; } = null!;

    [JsonProperty("article"), Required]
    public string Article { get; set; } = null!;
}
