using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record Tag
{
    [JsonPropertyName("name")]
    public required string Name { get; set; } = "";

    [JsonPropertyName("translated_name")]
    public required string? TranslatedName { get; set; }
}