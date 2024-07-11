using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Models.Articles;

public class ArticleAlias
{
    [BsonId]
    public string Alias { get; set; } = null!;

    [BsonElement("article")]
    public string Article { get; set; } = null!;
}
