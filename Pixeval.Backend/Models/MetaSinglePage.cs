using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public record MetaSinglePage
{
    /// <summary>
    /// ��ͼ���ͼʱ��ԭͼ����
    /// </summary>
    [JsonPropertyName("original_image_url")]
    public string? OriginalImageUrl { get; set; }
}