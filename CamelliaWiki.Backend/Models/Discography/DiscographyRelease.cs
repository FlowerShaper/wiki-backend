using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyRelease
{
    [BsonElement("year")]
    [JsonProperty("year")]
    public int Year { get; set; }

    [BsonElement("month")]
    [JsonProperty("month")]
    public int Month { get; set; }

    [BsonElement("day")]
    [JsonProperty("day")]
    public int Day { get; set; }
}
