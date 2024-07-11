using CamelliaWiki.Backend.Components;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public class DynamicsHelper
{
    private static IMongoCollection<DynamicData> collection => MongoDatabase.GetCollection<DynamicData>("dynamic");

    public static void Set(DynamicDataType key, string value)
    {
        var existing = get(key);

        if (existing is null)
        {
            collection.InsertOne(new DynamicData(key, value));
            return;
        }

        existing.Value = value;
        collection.ReplaceOne(x => x.Key == key, existing);
    }

    public static bool Exists(DynamicDataType key) => get(key) is not null;
    public static void Delete(DynamicDataType key) => collection.DeleteOne(x => x.Key == key);

    public static string? Get(DynamicDataType key) => get(key)?.Value;

    private static DynamicData? get(DynamicDataType key)
        => collection.Find(x => x.Key == key).FirstOrDefault();
}
