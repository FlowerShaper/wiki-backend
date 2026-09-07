using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CamelliaWiki.Backend.Models;

[Table("dynamic")]
public class DynamicData
{
    [Key, Column("key"), Required]
    public DynamicDataType Key { get; init; } = 0;

    [Column("value"), Required, MaxLength(512)]
    public string Value { get; set; } = "";

    public DynamicData(DynamicDataType key, string value)
    {
        Key = key;
        Value = value;
    }

    private DynamicData()
    {
    }
}

public enum DynamicDataType
{
    FeaturedPost = 0,
    PopularPost = 1,
    HomePosts = 2
}
