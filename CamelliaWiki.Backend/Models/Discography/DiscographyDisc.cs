using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyDisc
{
    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("tracks")]
    [JsonProperty("tracks")]
    public List<string> Tracks { get; set; } = new();
}
