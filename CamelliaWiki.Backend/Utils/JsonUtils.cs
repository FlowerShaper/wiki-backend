using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CamelliaWiki.Backend.Utils;

public static class JsonUtils
{
    public static T? Deserialize<T>(this string json) => JsonConvert.DeserializeObject<T>(json, globalSettings());
    public static string Serialize<T>(this T obj, bool indent = false) => JsonConvert.SerializeObject(obj, globalSettings(indent));

    private static JsonSerializerSettings globalSettings(bool indent = false) => new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = indent ? Formatting.Indented : Formatting.None,
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        NullValueHandling = NullValueHandling.Ignore
    };

    public static bool TryParse<T>(string json, out T? result)
    {
        try
        {
            result = JsonSerializer.Deserialize<T>(json);
            return result is not null;
        }
        catch (JsonException ex)
        {
            result = default;
            Logger.Log(ex);
            return false;
        }
    }
}
