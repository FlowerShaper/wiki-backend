using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Components.Articles;

[JsonObject(MemberSerialization.OptIn)]
public class Article
{
    [BsonId]
    public string ID { get; init; } = "";

    [BsonIgnore]
    [JsonProperty("lang")]
    public string Language => ID.Split(':')[1];

    [BsonElement("meta")]
    [JsonProperty("meta")]
    public ArticleMetadata Metadata { get; set; } = null!;

    [BsonElement("content")]
    [JsonProperty("content")]
    public string Content { get; set; } = "";
}
