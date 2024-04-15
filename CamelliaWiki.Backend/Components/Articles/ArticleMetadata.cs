using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Components.Articles;

[JsonObject(MemberSerialization.OptIn)]
public class ArticleMetadata
{
    [BsonElement("title")]
    [JsonProperty("title")]
    public string Title { get; set; } = "";

    [BsonElement("description")]
    [JsonProperty("description")]
    public string Description { get; set; } = "";

    [BsonElement("author")]
    [JsonProperty("author")]
    public string Author { get; set; } = "";

    [BsonElement("layout")]
    [JsonProperty("layout")]
    public string Layout { get; set; } = "";

    [BsonElement("date")]
    [JsonProperty("date")]
    public long Date { get; set; } = 0;
}
