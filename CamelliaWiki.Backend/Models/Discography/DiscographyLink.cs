using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyLink
{
    [BsonElement("label")]
    [JsonProperty("label")]
    public string Label { get; set; } = string.Empty;

    [BsonElement("url")]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
