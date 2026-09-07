using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Discography;

[Owned]
public class DiscographyRelease : IComparable<DiscographyRelease>
{
    [Column("year")]
    [JsonProperty("year")]
    public int? Year { get; set; }

    [Column("month")]
    [JsonProperty("month")]
    public int? Month { get; set; }

    [Column("day")]
    [JsonProperty("day")]
    public int? Day { get; set; }

    public int CompareTo(DiscographyRelease? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;

        var y = other.Year?.CompareTo(Year ?? 0) ?? 0;
        if (y != 0) return y;

        var m = other.Month?.CompareTo(Month ?? 0) ?? 0;
        if (m != 0) return m;

        return other.Day?.CompareTo(Day ?? 0) ?? 0;
    }
}
