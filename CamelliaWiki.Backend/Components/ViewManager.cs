using System.Net;
using CamelliaWiki.Backend.Components.Views;
using CamelliaWiki.Backend.Database;
using CamelliaWiki.Backend.Utils;
using Midori.Logging;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Components;

/// <summary>
/// component that handles per-ip views of posts
/// </summary>
public class ViewManager : IDisposable
{
    private IMongoCollection<ArticleView> collection => MongoDatabase.GetCollection<ArticleView>("views");

    private Logger logger { get; } = Logger.GetLogger("ViewManager");
    private Task cleanupTask { get; }
    private bool runCleanup = true;

    public ViewManager()
    {
        cleanupTask = Task.Run(() =>
        {
            // wait a little bit for everything
            Thread.Sleep(TimeSpan.FromSeconds(2));

            while (runCleanup)
            {
                cleanup();
                Thread.Sleep(TimeSpan.FromHours(12));
            }
        });
    }

    public void Log(IPAddress ip, string article)
    {
        var str = ip.ToString();

        // ignore localhost
        /*if (str is "127.0.0.1" or "::1")
            return;*/

        collection.InsertOne(new ArticleView(ip, article));
    }

    /// <summary>
    /// cleans up old data that we don't need/shouldn't have anymore
    /// </summary>
    private void cleanup()
    {
        logger.Add("cleaning...", LogLevel.Debug);

        try
        {
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var span = TimeSpan.FromDays(7).TotalSeconds;
            var result = collection.DeleteMany(x => now - x.Time > span);

            logger.Add($"cleaned {result.DeletedCount} view(s)!", LogLevel.Debug);
        }
        catch (Exception ex)
        {
            logger.Add("failed to clean!", LogLevel.Error, ex);
        }
    }

    public List<ArticleView> GetToday()
    {
        var now = DateTimeOffset.Now;
        var nowLong = now.ToUnixTimeSeconds();
        var start = now.StartOfDay();
        var span = (now - start).TotalSeconds;

        return collection.Find(x => nowLong - x.Time < span).ToList();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        runCleanup = false;
        cleanupTask.Dispose();
    }
}
