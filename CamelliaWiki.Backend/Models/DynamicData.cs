using MongoDB.Bson.Serialization.Attributes;

namespace CamelliaWiki.Backend.Models;

public class DynamicData
{
    [BsonId]
    public DynamicDataType Key { get; set; } = 0;

    [BsonElement("value")]
    public string Value { get; set; } = "";

    public DynamicData(DynamicDataType key, string value)
    {
        Key = key;
        Value = value;
    }

    [BsonConstructor]
    [Obsolete("Used for BSON only.")]
    public DynamicData()
    {
    }
}

public enum DynamicDataType
{
    FeaturedPost = 0,
    PopularPost = 1,
    HomePosts = 2
}
