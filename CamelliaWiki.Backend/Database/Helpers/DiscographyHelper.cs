using CamelliaWiki.Backend.Models.Discography;
using MongoDB.Driver;

namespace CamelliaWiki.Backend.Database.Helpers;

public static class DiscographyHelper
{
    private static IMongoCollection<DiscographyTrack> tracks => MongoDatabase.GetCollection<DiscographyTrack>("dcg-tracks");
    private static IMongoCollection<DiscographyAlbum> albums => MongoDatabase.GetCollection<DiscographyAlbum>("dcg-albums");

    public static List<DiscographyTrack> AllTracks => tracks.Find(_ => true).ToList();
    public static List<DiscographyAlbum> AllAlbums => albums.Find(_ => true).ToList();

    public static void AddTrack(DiscographyTrack track) => tracks.InsertOne(track);
    public static void AddAlbum(DiscographyAlbum album) => albums.InsertOne(album);

    public static DiscographyTrack? GetTrack(string id) => tracks.Find(t => t.ID == id).FirstOrDefault();
    public static DiscographyAlbum? GetAlbum(string id) => albums.Find(a => a.ID == id).FirstOrDefault();

    public static void Wipe()
    {
        tracks.DeleteMany(_ => true);
        albums.DeleteMany(_ => true);
    }
}
