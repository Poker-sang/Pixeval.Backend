using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

[DebuggerDisplay("{Id}: {Name}")]
public record User
{
    [Key]
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; } = "";

    [JsonPropertyName("account")]
    public required string Account { get; set; } = "";

    [JsonPropertyName("profile_image_urls")]
    [NotMapped]
    public required ProfileImageUrls ProfileImageUrls
    {
        get => new() { Medium = ProfileImageUrlsString };
        set => ProfileImageUrlsString = value.Medium;
    }

    [JsonIgnore]
    public string ProfileImageUrlsString { get; set; } = "";

    [JsonPropertyName("is_followed")]
    [NotMapped]
    public bool IsFollowed { get; set; }

    [NotMapped]
    [JsonIgnore]
    public List<Illustration> Posts { get; set; } = [];
}

