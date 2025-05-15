using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

[PrimaryKey(nameof(UserId), nameof(Tag), nameof(DateTime))]
public class ChatItem
{
    [JsonIgnore]
    public required long UserId { get; set; }

    [MaxLength(20)]
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    [JsonPropertyName("dateTime")]
    public required DateTime DateTime { get; set; }

    [JsonPropertyName("text")]
    public required string Text { get; set; } = "";

    [JsonPropertyName("user")]
    public UserEntity User { get; set; } = null!;
}
