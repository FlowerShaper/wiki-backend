using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models;

public class Statistics
{
    [JsonProperty("articles")]
    public long Articles { get; set; }

    [JsonProperty("comments")]
    public long Comments { get; set; }

    [JsonProperty("visitors")]
    public long UniqueVisitors { get; set; }
}
