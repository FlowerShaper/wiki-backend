using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public class Config
{
    [JsonProperty("token")]
    public string Token { get; set; } = "";

    [JsonProperty("mongo")]
    public string MongoStr { get; set; } = "mongodb://localhost:27017";

    [JsonProperty("host")]
    public string Host { get; set; } = "";

    [JsonProperty("data-dir")]
    public string DataDirectory { get; set; } = "";
}
