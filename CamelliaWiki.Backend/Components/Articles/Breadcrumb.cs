using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Components.Articles;

public class Breadcrumb
{
    [BsonElement("name")]
    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [BsonElement("path")]
    [JsonProperty("path")]
    public string Path { get; set; } = "";
}
