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

    [BsonElement("type")]
    [JsonProperty("type")]
    public ArticleType Type { get; set; } = ArticleType.Article;

    [BsonElement("date")]
    [JsonProperty("date")]
    public long Date { get; set; } = 0;
}

public enum ArticleType
{
    /// <summary>
    /// Your typical wiki article.
    /// </summary>
    Article = 1,

    /// <summary>
    /// News about wiki updates or similar.
    /// </summary>
    News = 2,

    /// <summary>
    /// Community-written blog posts.
    /// </summary>
    Community = 3
}
