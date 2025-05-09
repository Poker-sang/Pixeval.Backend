using System.Text.Json;
using Pixeval.Backend.Models;

namespace Pixeval.Backend.Services;

public static partial class TempService
{
    public static async Task InitIllustrationsUsersAsync(IServiceProvider provider)
    {
        try
        {
            using var serviceScope = provider.CreateScope();
            await using var pixevalDbContext = serviceScope.ServiceProvider.GetRequiredService<PixevalDbContext>();

            IReadOnlyList<Illustration> list;
            await using (var fileStream = StaticContext.OpenAsyncReadContext("record.json"))
                list = (await JsonSerializer.DeserializeAsync<IReadOnlyList<Illustration>>(fileStream))!;

            var me = JsonSerializer.Deserialize<UserEntity>(Me)!;
            _ = await pixevalDbContext.Users.AddAsync(me);

            var i = 0;
            foreach (var illustration in list)
            {
                Console.WriteLine(i);
                ++i;
                if (await pixevalDbContext.Illustrations.FindAsync(illustration.Id) is not null)
                    continue;

                if (await illustration.Title.TranslateTextAsync() is { } a)
                    illustration.Title = a;

                if (await illustration.Description.TranslateTextAsync() is { } b)
                    illustration.Description = b;

                foreach (var illustrationTag in illustration.Tags)
                    if (illustrationTag.TranslatedName is { } translatedName
                        && await translatedName.TranslateTextAsync() is { } c
                        && c == translatedName)
                    {
                        illustrationTag.Name = c;
                        illustrationTag.TranslatedName = null;
                    }
                    else if (await illustrationTag.Name.TranslateTextAsync() is { } d)
                    {
                        illustrationTag.Name = d;
                        illustrationTag.TranslatedName = null;
                    }

                illustration.Tags = illustration.Tags;

                if (await pixevalDbContext.Users.FindAsync(illustration.User.Id) is { } existingUser)
                    illustration.User = existingUser;

                var text = await File.ReadAllTextAsync(StaticContext.GetContextFile($"img\\{illustration.Id}.json"));
                illustration.Tags2String = text;

                _ = await pixevalDbContext.Illustrations.AddAsync(illustration);
            }

            await using var s = StaticContext.OpenAsyncWriteContext("record2.json");
            await JsonSerializer.SerializeAsync(s, list);

            await pixevalDbContext.SaveChangesAsync();
        }
        finally
        {
            await TranslateService.SerializeAsync();
        }
    }

    private const string Me =
        """{"id":104757871,"name":"Test","account":"user_hscv7227","profile_image_urls":{"medium":"https:\/\/s.pximg.net\/common\/images\/no_profile.png"},"comment":"","is_followed":false,"is_access_blocking_user":false}""";

    private const long MyId = 104757871;
}
