using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyCredit
{
    [BsonElement("role")]
    [JsonProperty("role")]
    public string Role { get; set; } = string.Empty;

    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
