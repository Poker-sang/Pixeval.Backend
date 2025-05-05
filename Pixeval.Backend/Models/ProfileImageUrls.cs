using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record ProfileImageUrls
{
    [JsonPropertyName("medium")]
    public required string Medium { get; set; } = DefaultImageUrls.ImageNotAvailable;
}