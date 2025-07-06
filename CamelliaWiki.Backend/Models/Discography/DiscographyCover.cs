using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyCover
{
    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("url")]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;
}
