using CamelliaWiki.Backend.Models;
using CamelliaWiki.Backend.Models.Articles;
using CamelliaWiki.Backend.Models.Characters;
using CamelliaWiki.Backend.Models.Comments;
using CamelliaWiki.Backend.Models.Discography;
using CamelliaWiki.Backend.Models.Users;
using Microsoft.EntityFrameworkCore;
using Midori.Utils;

namespace CamelliaWiki.Backend.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Article> Articles { get; }
    public DbSet<ArticleMetadata> ArticleMeta { get; }
    public DbSet<ArticleAlias> Aliases { get; }
    public DbSet<Character> Characters { get; }
    public DbSet<Comment> Comments { get; }
    public DbSet<DiscographyAlbum> Albums { get; }
    public DbSet<DiscographyTrack> Tracks { get; }
    public DbSet<DynamicData> Dynamics { get; }
    public DbSet<User> Users { get; }

    public DatabaseContext(DbContextOptions<DatabaseContext> opt)
        : base(opt)
    {
        Articles = Set<Article>();
        ArticleMeta = Set<ArticleMetadata>();
        Aliases = Set<ArticleAlias>();
        Characters = Set<Character>();
        Comments = Set<Comment>();
        Albums = Set<DiscographyAlbum>();
        Tracks = Set<DiscographyTrack>();
        Dynamics = Set<DynamicData>();
        Users = Set<User>();
    }

    protected override void OnModelCreating(ModelBuilder build)
    {
        base.OnModelCreating(build);

        build.Entity<Article>()
             .Property(x => x.Language).HasConversion<string>();

        build.Entity<ArticleMetadata>()
             .Property(x => x.Language).HasConversion<string>();

        build.Entity<Character>(b =>
        {
            b.ToTable("character");
            b.OwnsMany(c => c.Images, o =>
            {
                o.WithOwner().HasForeignKey("character");
                o.ToTable("character-image");
            });
        });

        build.Entity<DiscographyAlbum>(b =>
        {
            b.ToTable("album");
            b.OwnsOne(a => a.Release, o =>
            {
                o.WithOwner().HasForeignKey("album");
                o.ToTable("album-release");
            });
            b.OwnsMany(a => a.Covers, o =>
            {
                o.WithOwner().HasForeignKey("album");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("album-cover");
            });
            b.OwnsMany(a => a.Discs, o =>
            {
                o.WithOwner().HasForeignKey("album");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("album-disc");
            });
            b.OwnsMany(a => a.Credits, o =>
            {
                o.WithOwner().HasForeignKey("album");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("album-credit");
            });
            b.OwnsMany(a => a.Links, o =>
            {
                o.WithOwner().HasForeignKey("album");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("album-link");
            });
        });

        build.Entity<DiscographyTrack>(b =>
        {
            b.ToTable("track");
            b.OwnsOne(t => t.Release, o =>
            {
                o.WithOwner().HasForeignKey("track");
                o.ToTable("track-release");
            });
            b.OwnsMany(t => t.Covers, o =>
            {
                o.WithOwner().HasForeignKey("track");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("track-cover");
            });
            b.OwnsMany(t => t.Credits, o =>
            {
                o.WithOwner().HasForeignKey("track");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("track-credit");
            });
            b.OwnsMany(t => t.Links, o =>
            {
                o.WithOwner().HasForeignKey("track");
                o.Property<int>("Id").HasColumnName("id");
                o.ToTable("track-link");
            });
        });
    }

    public IDisposable EditAndSave() => new InvokeOnDisposal(() => SaveChanges());
}
