using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record ImageUrls
{
    [JsonPropertyName("square_medium")]
    public required string SquareMedium { get; set; } = DefaultImageUrls.ImageNotAvailable;

    [JsonPropertyName("medium")]
    public required string Medium { get; set; } = DefaultImageUrls.ImageNotAvailable;

    [JsonPropertyName("large")]
    public required string Large { get; set; } = DefaultImageUrls.ImageNotAvailable;
}
