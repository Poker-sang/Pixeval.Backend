using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pixeval.Backend.Models;

public class Illustration
{
    [JsonPropertyName("type")]
    [JsonConverter(typeof(JsonStringEnumConverter<IllustrationType>))]
    public required IllustrationType Type { get; set; }

    [JsonPropertyName("tools")]
    [NotMapped]
    public IReadOnlyList<string> Tools => [];

    [JsonPropertyName("page_count")]
    public required int PageCount { get; set; }

    [JsonPropertyName("width")]
    public required int Width { get; set; }

    [JsonPropertyName("height")]
    public required int Height { get; set; }

    [JsonPropertyName("sanity_level")]
    public required int SanityLevel { get; set; }

    [JsonPropertyName("illust_ai_type")]
    public required AiType AiType { get; set; }

    [JsonPropertyName("illust_book_style")]
    public required int IllustBookStyle { get; set; }

    [Key]
    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; } = "";

    [JsonPropertyName("caption")]
    public required string Description { get; set; } = "";

    [JsonPropertyName("restrict")]
    [JsonConverter(typeof(BoolToNumberJsonConverter))]
    public required bool IsPrivate { get; set; }

    [JsonPropertyName("x_restrict")]
    public required XRestrict XRestrict { get; set; }

    [JsonPropertyName("tags")]
    [NotMapped]
    [field: AllowNull, MaybeNull]
    public required IReadOnlyList<Tag> Tags
    {
        get => field ??= JsonSerializer.Deserialize<IReadOnlyList<Tag>>(TagsString)!;
        set => TagsString = JsonSerializer.Serialize(field = value);
    }

    [JsonIgnore]
    public string TagsString { get; set; } = "";

    [JsonPropertyName("tags2")]
    [NotMapped]
    [field: AllowNull, MaybeNull]
    public IReadOnlyList<string> Tags2 => field ??= JsonSerializer.Deserialize<TagPredPairList>(Tags2String)!.GeneralRes.Keys.ToArray();

    [JsonIgnore]
    [NotMapped]
    [field: AllowNull, MaybeNull]
    public TagPredPairList Tags3
    {
        get => field ??= JsonSerializer.Deserialize<TagPredPairList>(Tags2String)!;
        set => Tags2String = JsonSerializer.Serialize(field = value);
    }

    [JsonIgnore]
    public string Tags2String { get; set; } = "";

    [JsonIgnore]
    public long UserId { get; set; }

    [NotMapped]
    [JsonPropertyName("user")]
    public required User User
    {
        get;
        set
        {
            field = value;
            UserId = value.Id;
        }
    }

    [JsonPropertyName("create_date")]
    public required DateTimeOffset CreateDate { get; set; }

    [JsonPropertyName("image_urls")]
    [NotMapped]
    public required ImageUrls ThumbnailUrls
    {
        get => JsonSerializer.Deserialize<ImageUrls>(ThumbnailUrlsString)!;
        set => ThumbnailUrlsString = JsonSerializer.Serialize(value);
    }

    [JsonIgnore]
    public string ThumbnailUrlsString { get; set; } = "";

    [NotMapped]
    [JsonPropertyName("is_bookmarked")]
    public bool IsFavorite { get; set; }

    [JsonPropertyName("total_bookmarks")]
    public required int TotalFavorite { get; set; }

    [JsonPropertyName("total_view")]
    public required int TotalView { get; set; }

    [JsonPropertyName("visible")]
    [NotMapped]
    public bool Visible => true;

    [JsonPropertyName("is_muted")]
    [NotMapped]
    public bool IsMuted => false;

    [JsonPropertyName("series")]
    [NotMapped]
    public object? Series => null;

    [JsonPropertyName("meta_single_page")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [NotMapped]
    public required MetaSinglePage MetaSinglePage
    {
        get => new() { OriginalImageUrl = MetaSinglePageString };
        set => MetaSinglePageString = value.OriginalImageUrl;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [JsonIgnore]
    public string? MetaSinglePageString { get; set; }

    [JsonPropertyName("meta_pages")]
    [NotMapped]
    [field: AllowNull, MaybeNull]
    public required IReadOnlyList<MetaPage> MetaPages
    {
        get => field ??= JsonSerializer.Deserialize<IReadOnlyList<MetaPage>>(MetaPagesString)!;
        set => MetaPagesString = JsonSerializer.Serialize(field = value);
    }

    [JsonIgnore]
    public string MetaPagesString { get; set; } = "";
}
