using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Models.Articles;

public class ArticleAlias
{
    [BsonId]
    public string Alias { get; init; } = null!;

    [BsonElement("article")]
    public string Article { get; init; } = null!;
}
