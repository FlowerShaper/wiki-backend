using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Articles;

public class Breadcrumb
{
    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [BsonElement("path")]
    [JsonProperty("path")]
    public string Path { get; set; } = "";
}
