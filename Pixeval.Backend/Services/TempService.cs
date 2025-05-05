namespace Pixeval.Backend.Services;

public static partial class TempService
{
    public static async Task<bool> InitDbContextAsync(IServiceProvider provider)
    {
        using var serviceScope = provider.CreateScope();
        await using var pixevalDbContext = serviceScope.ServiceProvider.GetRequiredService<PixevalDbContext>();
        return await pixevalDbContext.Database.EnsureCreatedAsync();
    }
}
