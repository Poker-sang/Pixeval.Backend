using System.Text.Json;

namespace Pixeval.Backend.Services;

public static partial class TempService
{
    public static async Task InitFavoriteAsync(IServiceProvider provider)
    {
        using var serviceScope = provider.CreateScope();
        await using var pixevalDbContext = serviceScope.ServiceProvider.GetRequiredService<PixevalDbContext>();

        IReadOnlyList<long> list;
        await using (var fileStream = StaticContext.OpenAsyncReadContext("favor.json"))
            list = (await JsonSerializer.DeserializeAsync<IReadOnlyList<long>>(fileStream))!;

        foreach (var id in list)
            _ = await pixevalDbContext.FavoriteList.AddAsync(new()
            {
                DateTime = DateTime.UtcNow - TimeSpan.FromHours(Random.Shared.Next(240)),
                IllustrationId = id,
                UserId = MyId,
            });
        await pixevalDbContext.SaveChangesAsync();
    }
}
