using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

[PrimaryKey(nameof(UserId), nameof(IllustrationId), nameof(DateTime))]
public class CommentItem
{
    [JsonIgnore]
    public required long UserId { get; set; }

    [JsonIgnore]
    public required long IllustrationId { get; set; }

    [JsonPropertyName("dateTime")]
    public required DateTime DateTime { get; set; }

    [JsonPropertyName("comment")]
    public required string Comment { get; set; } = "";

    [JsonPropertyName("user")]
    public UserEntity User { get; set; } = null!;

    [JsonIgnore]
    public Illustration Illustration { get; set; } = null!;
}
