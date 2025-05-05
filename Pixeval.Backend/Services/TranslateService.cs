using System.Net;
using System.Text.Json;

namespace Pixeval.Backend.Services;

public static class TranslateService
{
    static TranslateService()
    {
        using var fs = StaticContext.OpenAsyncReadContext("trans.json");
        _Trans = JsonSerializer.Deserialize<Dictionary<string, string>>(fs)!;
    }

    private static readonly Dictionary<string, string> _Trans;

    private static DateTime _Last;

    public static async Task<string?> TranslateTextAsync(this string input, Languages sl = Languages.Auto, Languages tl = Languages.Cn)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        if (_Trans.TryGetValue(input, out var value))
            return value;
        Start:
        try
        {
            var now = DateTime.Now;
            if (now < _Last + TimeSpan.FromMilliseconds(10))
                await Task.Delay(10);
            _Last = now;

            var sourceLang = sl is Languages.Cn ? "zh-CN" : sl.ToString().ToLowerInvariant();
            var toLang = tl is Languages.Cn ? "zh-CN" : tl.ToString().ToLowerInvariant();
            // Set the language from/to in the url (or pass it into this function)
            var result =
                await
                    $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLang}&tl={toLang}&dt=t&q={Uri.EscapeDataString(input)}"
                        .DownloadStringAsync();

            // Get all json data

            if (JsonSerializer.Deserialize<JsonElement[]>(result) is not { } jsonData)
                return null;

            // Extract just the first array element (This is the only data we are interested in)
            var translationItems = jsonData[0];

            // Translation Data
            // Loop through the collection extracting the translated objects
            var translation = string.Join(' ', translationItems.EnumerateArray());

            if (jsonData[2].GetString() is "zh-CN")
            {
                _Trans[input] = input;
                return input;
            }

            // Remove first blank character
            if (translation.Length > 1)
                translation = translation[1..];
            else
                return null;

            // Return translation

            _Trans[input] = translation;
            return translation;
        }
        catch (HttpRequestException e) when (e.StatusCode is HttpStatusCode.InternalServerError)
        {
            await SerializeAsync();
            await Task.Delay(60000);
            goto Start;
        }
        catch (Exception)
        {
            await SerializeAsync();
            return null;
        }
    }

    public static async Task SerializeAsync()
    {
        await using var fs = StaticContext.OpenAsyncWriteContext("trans.json");
        await JsonSerializer.SerializeAsync(fs, _Trans);
    }

    public enum Languages
    {
        Auto,
        Cn,
        En,
        Ja,
        De,
        Ru,
        Fr,
        It,
        Ko,
        Ar,
        Es
    }
}
