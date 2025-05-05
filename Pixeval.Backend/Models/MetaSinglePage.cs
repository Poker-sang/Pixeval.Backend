using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record MetaSinglePage
{
    /// <summary>
    /// 单图或多图时的原图链接
    /// </summary>
    [JsonPropertyName("original_image_url")]
    public string? OriginalImageUrl { get; set; }
}