using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CamelliaWiki.Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "album",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    title_romanized = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "alias",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    article = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    path = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    lang = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => new { x.path, x.lang });
                });

            migrationBuilder.CreateTable(
                name: "character",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dynamic",
                columns: table => new
                {
                    key = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dynamic", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "track",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    title_romanized = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    length = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    bpm = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    single = table.Column<bool>(type: "boolean", nullable: false),
                    albums = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    avatar = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    banner = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    join = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "album-cover",
                columns: table => new
                {
                    album = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album-cover", x => new { x.album, x.id });
                    table.ForeignKey(
                        name: "FK_album-cover_album_album",
                        column: x => x.album,
                        principalTable: "album",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album-credit",
                columns: table => new
                {
                    album = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album-credit", x => new { x.album, x.id });
                    table.ForeignKey(
                        name: "FK_album-credit_album_album",
                        column: x => x.album,
                        principalTable: "album",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album-disc",
                columns: table => new
                {
                    album = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    tracks = table.Column<string[]>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album-disc", x => new { x.album, x.id });
                    table.ForeignKey(
                        name: "FK_album-disc_album_album",
                        column: x => x.album,
                        principalTable: "album",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album-link",
                columns: table => new
                {
                    album = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album-link", x => new { x.album, x.id });
                    table.ForeignKey(
                        name: "FK_album-link_album_album",
                        column: x => x.album,
                        principalTable: "album",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "album-release",
                columns: table => new
                {
                    album = table.Column<string>(type: "character varying(256)", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: true),
                    month = table.Column<int>(type: "integer", nullable: true),
                    day = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_album-release", x => x.album);
                    table.ForeignKey(
                        name: "FK_album-release_album_album",
                        column: x => x.album,
                        principalTable: "album",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article-meta",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    lang = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    layout = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    date = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article-meta", x => new { x.id, x.lang });
                    table.ForeignKey(
                        name: "FK_article-meta_article_id_lang",
                        columns: x => new { x.id, x.lang },
                        principalTable: "article",
                        principalColumns: new[] { "path", "lang" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character-image",
                columns: table => new
                {
                    character = table.Column<string>(type: "character varying(128)", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    src = table.Column<string>(type: "text", nullable: false),
                    alt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character-image", x => new { x.character, x.Id });
                    table.ForeignKey(
                        name: "FK_character-image_character_character",
                        column: x => x.character,
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "track-cover",
                columns: table => new
                {
                    track = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track-cover", x => new { x.track, x.id });
                    table.ForeignKey(
                        name: "FK_track-cover_track_track",
                        column: x => x.track,
                        principalTable: "track",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "track-credit",
                columns: table => new
                {
                    track = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track-credit", x => new { x.track, x.id });
                    table.ForeignKey(
                        name: "FK_track-credit_track_track",
                        column: x => x.track,
                        principalTable: "track",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "track-link",
                columns: table => new
                {
                    track = table.Column<string>(type: "character varying(256)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track-link", x => new { x.track, x.id });
                    table.ForeignKey(
                        name: "FK_track-link_track_track",
                        column: x => x.track,
                        principalTable: "track",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "track-release",
                columns: table => new
                {
                    track = table.Column<string>(type: "character varying(256)", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: true),
                    month = table.Column<int>(type: "integer", nullable: true),
                    day = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_track-release", x => x.track);
                    table.ForeignKey(
                        name: "FK_track-release_track_track",
                        column: x => x.track,
                        principalTable: "track",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    author = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    slug = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    content = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    edited = table.Column<long>(type: "bigint", nullable: false),
                    parent = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_comment_parent",
                        column: x => x.parent,
                        principalTable: "comment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_comment_user_author",
                        column: x => x.author,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment-vote",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    user = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    value = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment-vote", x => new { x.id, x.user });
                    table.ForeignKey(
                        name: "FK_comment-vote_comment_id",
                        column: x => x.id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_author",
                table: "comment",
                column: "author");

            migrationBuilder.CreateIndex(
                name: "IX_comment_parent",
                table: "comment",
                column: "parent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "album-cover");

            migrationBuilder.DropTable(
                name: "album-credit");

            migrationBuilder.DropTable(
                name: "album-disc");

            migrationBuilder.DropTable(
                name: "album-link");

            migrationBuilder.DropTable(
                name: "album-release");

            migrationBuilder.DropTable(
                name: "alias");

            migrationBuilder.DropTable(
                name: "article-meta");

            migrationBuilder.DropTable(
                name: "character-image");

            migrationBuilder.DropTable(
                name: "comment-vote");

            migrationBuilder.DropTable(
                name: "dynamic");

            migrationBuilder.DropTable(
                name: "track-cover");

            migrationBuilder.DropTable(
                name: "track-credit");

            migrationBuilder.DropTable(
                name: "track-link");

            migrationBuilder.DropTable(
                name: "track-release");

            migrationBuilder.DropTable(
                name: "album");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "character");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "track");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
