using CamelliaWiki.Backend.Utils;
using Midori.Searching;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Articles;

[JsonObject(MemberSerialization.OptIn)]
public class Article : IComparable<Article>
{
    [BsonId]
    public string ID { get; init; } = "";

    [BsonIgnore]
    [JsonProperty("url")]
    [Searchable("path")]
    public string Path => ID.Split(':')[0];

    [BsonIgnore]
    public Language Language => LanguageUtils.TryParse(LanguageCode, out var lang) ? lang : Language.en;

    [BsonIgnore]
    [JsonProperty("lang")]
    public string LanguageCode => ID.Split(':')[1];

    [BsonElement("meta")]
    [JsonProperty("meta")]
    public ArticleMetadata Metadata { get; set; } = null!;

    [BsonElement("breadcrumbs")]
    [JsonProperty("breadcrumbs")]
    public List<Breadcrumb> Breadcrumbs { get; set; } = new();

    [BsonElement("content")]
    [JsonProperty("content")]
    public string Content { get; set; } = "";

    public int CompareTo(Article? other)
    {
        if (ReferenceEquals(this, other))
            return 0;

        if (ReferenceEquals(null, other))
            return 1;

        return Metadata.Date.CompareTo(other.Metadata.Date);
    }

    #region Searching

    [BsonIgnore]
    [Searchable("title")]
    [Obsolete("Use Metadata.Title instead.")]
    public string SearchableTitle => Metadata.Title;

    [BsonIgnore]
    [Searchable("description")]
    [Obsolete("Use Metadata.Description instead.")]
    public string SearchableDescription => Metadata.Description;

    #endregion
}
