using System.Net;
using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Components.Views;

[BsonNoId]
public class ArticleView
{
    [BsonElement("ip")]
    public string IP { get; set; } = null!;

    [BsonElement("article")]
    public string Article { get; set; } = null!;

    [BsonElement("time")]
    public long Time { get; set; }

    public ArticleView(IPAddress ip, string article)
    {
        IP = ip.ToString();
        Article = article;
        Time = DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    [BsonConstructor]
    [Obsolete("Used for BSON parsing.")]
    public ArticleView()
    {
    }
}
