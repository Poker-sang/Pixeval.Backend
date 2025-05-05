namespace Pixeval.Backend.Services;

public static partial class TempService
{
    public static async Task InitFollowAsync(IServiceProvider provider)
    {
        using var serviceScope = provider.CreateScope();
        await using var pixevalDbContext = serviceScope.ServiceProvider.GetRequiredService<PixevalDbContext>();

        var dict = pixevalDbContext.FavoriteList.Select(t => t.Illustration.UserId).GroupBy(t => t)
            .ToDictionary(t => t.Key, t => t.Count());

        foreach (var (user, count) in dict)
            if (count > 1)
                _ = await pixevalDbContext.FollowList.AddAsync(new()
                {
                    FollowedUserId = user,
                    UserId = MyId,
                });
        await pixevalDbContext.SaveChangesAsync();
    }
}
