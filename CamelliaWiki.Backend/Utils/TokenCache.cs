using System.Net.Http.Headers;
using Midori.Logging;
using Newtonsoft.Json.Linq;

namespace CamelliaWiki.Backend.Utils;

public static class TokenCache
{
    private static readonly object token_lock = new();
    private static Dictionary<string, ulong> tokens { get; } = new();

    public static bool TryGet(string? token, out ulong id)
    {
        lock (token_lock)
        {
            id = 0;

            if (string.IsNullOrWhiteSpace(token))
                return false;

            if (tokens.TryGetValue(token, out var cachedToken))
            {
                id = cachedToken;
                return true;
            }

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = client.GetAsync("https://discord.com/api/users/@me");
                var content = response.Result.Content.ReadAsStringAsync();
                var json = JObject.Parse(content.Result);

                if (json["id"] == null)
                    return false;

                var uid = json["id"]!.Value<string>()!;

                id = ulong.Parse(uid);

                tokens.Add(token, id);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to fetch user!", LoggingTarget.Network);
                return false;
            }
        }
    }
}
