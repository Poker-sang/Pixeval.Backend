using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace Pixeval.Backend.Services;

public static class HttpClientService
{
    public static void Initialize() { }

    public static HttpClient Client { get; } = new(new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.All,
        // ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    })
    {
        Timeout = new(0, 0, 0, 8000),
        MaxResponseContentBufferSize = ((long)2 << 30) - 1
    };

    private static bool _ShouldRefreshHeader = true;

    public static HttpClient InitializeHeader(this HttpClient client, Dictionary<string, string>? header = null)
    {
        if (!_ShouldRefreshHeader && header is null)
            return client;

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.UserAgent.Add(new("Mozilla", "5.0"));
        client.DefaultRequestHeaders.UserAgent.Add(new("PokerKagami", "1.0"));
        _ShouldRefreshHeader = false;
        if (header is not null)
        {
            _ShouldRefreshHeader = true;
            foreach (var (k, v) in header)
                client.DefaultRequestHeaders.Add(k, v);
        }

        Debug.WriteLine($"[{nameof(HttpClientService)}]::{nameof(InitializeHeader)}(): Header: [");
        foreach (var i in client.DefaultRequestHeaders)
        {
            Debug.WriteLine($"  {i.Key}:{string.Join(';', i.Value)},");
        }

        Debug.WriteLine($"]");
        return client;
    }

    public static Task<string> DownloadStringAsync(this string uri, Dictionary<string, string>? header = null)
        => Client.InitializeHeader(header).GetStringAsync(uri);

    public static Task<Stream> DownloadStreamAsync(this string uri, Dictionary<string, string>? header = null)
        => Client.InitializeHeader(header).GetStreamAsync(uri);

    public static Task<byte[]> DownloadBytesAsync(this string uri, Dictionary<string, string>? header = null)
        => Client.InitializeHeader(header).GetByteArrayAsync(uri);

    public static async Task<JsonDocument> DownloadJsonAsync(this string uri, Dictionary<string, string>? header = null)
    {
        await using var download = await uri.DownloadStreamAsync(header);
        return await JsonDocument.ParseAsync(download);
    }
}