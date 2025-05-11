using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController(ILogger<PostController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpPost]
    public async Task PostAsync(long userId, string title, string desc, string path, int width, int height)
    {
        var id = Random.Shared.NextInt64(130000000, long.MaxValue);
        var user = (await dbContext.Users.FindAsync(userId))!;
        var illust = new Illustration
        {
            Title = title,
            Description = desc,
            Type = IllustrationType.Illust,
            PageCount = 1,
            Width = width,
            Height = height,
            SanityLevel = 2,
            AiType = AiType.NotAiGenerated,
            IllustBookStyle = 0,
            IsPrivate = false,
            XRestrict = XRestrict.Ordinary,
            Id = id,
            Tags = [],
            Tags3 = new([], [], []),
            User = user,
            CreateDateOffset = DateTimeOffset.Now,
            ThumbnailUrls = new()
            {
                Large = path,
                Medium = path,
                SquareMedium = path
            },
            TotalFavorite = 0,
            TotalView = 0,
            MetaSinglePage = new() { OriginalImageUrl = path },
            MetaPages = []
        };
        await dbContext.Illustrations.AddAsync(illust);
        await dbContext.SaveChangesAsync();
    }
}
