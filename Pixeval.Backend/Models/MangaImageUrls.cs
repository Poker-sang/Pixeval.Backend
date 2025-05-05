using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record MangaImageUrls : ImageUrls
{
    /// <summary>
    /// ��ͼʱ��ԭͼ����
    /// </summary>
    [JsonPropertyName("original")]
    public required string Original { get; set; } = DefaultImageUrls.ImageNotAvailable;
}