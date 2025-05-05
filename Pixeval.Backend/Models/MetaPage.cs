using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record MetaPage
{
    [JsonPropertyName("image_urls")]
    public required MangaImageUrls ImageUrls { get; set; }
}