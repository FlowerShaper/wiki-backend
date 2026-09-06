using Newtonsoft.Json;

namespace CamelliaWiki.Backend;

public class Config
{
    [JsonProperty("token")]
    public string DiscordToken { get; set; } = "";

    [JsonProperty("admins")]
    public ulong[] AdminIDs { get; set; } = [];

    [JsonProperty("pgsql")]
    public string PostgresConnection { get; set; } = "";

    [JsonProperty("mongo")]
    public string MongoStr { get; set; } = "mongodb://localhost:27017";

    [JsonProperty("data-dir")]
    public string DataDirectory { get; set; } = "";
}
