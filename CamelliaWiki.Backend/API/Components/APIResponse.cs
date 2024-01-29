using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Components;

public class APIResponse
{
    /// <summary>
    /// The status code of the response. 0 means success.
    /// </summary>
    [JsonIgnore]
    public ErrorCodes Code { get; init; } = ErrorCodes.None;

    /// <summary>
    /// The status code in integer form.
    /// </summary>
    [JsonProperty("code")]
    public int ErrorCode => (int)Code;

    /// <summary>
    /// The data of the response.
    /// </summary>
    [JsonProperty("data")]
    public object? Data { get; init; }
}
