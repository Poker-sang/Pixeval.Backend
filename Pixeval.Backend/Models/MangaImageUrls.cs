using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record MangaImageUrls : ImageUrls
{
    /// <summary>
    /// 多图时的原图链接
    /// </summary>
    [JsonPropertyName("original")]
    public required string Original { get; set; } = DefaultImageUrls.ImageNotAvailable;
}