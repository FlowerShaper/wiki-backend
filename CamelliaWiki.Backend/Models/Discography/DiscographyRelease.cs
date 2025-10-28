using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

public class DiscographyRelease : IComparable<DiscographyRelease>
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

    public int CompareTo(DiscographyRelease? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;

        var y = other.Year.CompareTo(Year);
        if (y != 0) return y;

        var m = other.Month.CompareTo(Month);
        if (m != 0) return m;

        return other.Day.CompareTo(Day);
    }
}
