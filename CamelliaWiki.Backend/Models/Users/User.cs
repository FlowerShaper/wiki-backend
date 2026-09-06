using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CamelliaWiki.Backend.Models.Users;

/// <summary>
/// A user. Database-stored users are only used in fall-back scenarios where we can't get the user from Discord.
/// </summary>
[Table("user")]
public class User
{
    #region Stored

    /// <summary>
    /// The user's ID.
    /// </summary>
    [Key, Column("id"), Required]
    [JsonIgnore]
    public ulong ID { get; init; }

    /// <summary>
    /// The user's username.
    /// </summary>
    [Column("name"), MaxLength(32)]
    [JsonProperty("name")]
    public string Username { get; set; } = "";

    /// <summary>
    /// The url of the user's avatar.
    /// </summary>
    [Column("avatar"), MaxLength(128)]
    [JsonProperty("avatar")]
    public string AvatarUrl { get; set; } = "";

    /// <summary>
    /// The url of the user's banner.
    /// </summary>
    [Column("banner"), MaxLength(128)]
    [JsonProperty("banner")]
    public string BannerUrl { get; set; } = "";

    #endregion

    [NotMapped]
    [JsonProperty("id")]
    public string StringID => ID.ToString();

    [NotMapped]
    [JsonProperty("nick")]
    public string Nickname { get; set; } = "";

    [NotMapped]
    [JsonProperty("color")]
    public string RoleColor { get; set; } = "";

    [NotMapped]
    [JsonProperty("staff")]
    public bool IsStaff { get; set; } = false;

    [NotMapped]
    [JsonProperty("admin")]
    public bool IsAdmin => Program.Config.AdminIDs.Contains(ID);

    [Column("join")]
    [JsonProperty("join")]
    public long JoinDate { get; set; } = 0;

    /*[NotMapped]
    [JsonProperty("comments")]
    public int CommentCount => CommentHelper.FromUser(ID).Count;*/
}
