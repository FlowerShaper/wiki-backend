using System.Net;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.API.Components;

public class APIResponse
{
    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    [JsonProperty("status")]
    public HttpStatusCode Status { get; init; } = HttpStatusCode.OK;

    /// <summary>
    /// The message of the response.
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; init; } = "OK";

    /// <summary>
    /// The data of the response.
    /// </summary>
    [JsonProperty("data")]
    public object Data { get; init; } = new();
}
