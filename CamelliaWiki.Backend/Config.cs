using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public class Config
{
    [JsonProperty("token")]
    public string Token { get; set; } = "";

    [JsonProperty("host")]
    public string Host { get; set; } = "";
}
