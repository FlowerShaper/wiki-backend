using System.Net;
using CamelliaWiki.Backend.Database;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Components;

public class VisitorManager
{
    private IMongoCollection<UniqueVisitor> collection => MongoDatabase.GetCollection<UniqueVisitor>("visitors");

    public long TotalCount => collection.CountDocuments(x => true);

    public void Add(IPAddress ip)
    {
        var str = ip.ToString();

        // ignore localhost
        if (str is "127.0.0.1" or "::1")
            return;

        var existing = collection.Find(x => x.IP == str).FirstOrDefault();

        if (existing is not null)
            return;

        collection.InsertOne(new UniqueVisitor(ip));
    }

    public class UniqueVisitor
    {
        [BsonId]
        public string IP { get; set; } = null!;

        public UniqueVisitor(IPAddress ip)
        {
            IP = ip.ToString();
        }

        [BsonConstructor]
        [Obsolete("Used for BSON parsing.")]
        public UniqueVisitor()
        {
        }
    }
}
