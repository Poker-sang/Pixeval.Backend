using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;
using Pixeval.Backend.Services;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FavoriteController(ILogger<FavoriteController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IQueryable<Illustration>> ListAsync(long userId)
    {
        return await dbContext.FavoriteList.Where(t => t.UserId == userId)
            .Include(t => t.User)
            .OrderByDescending(t => t.DateTime)
            .Select(t => t.Illustration)
            .SelfForEachAsync(t => t.IsFavorite = true);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(long userId, long illustrationId, bool favorite)
    {
        if (await dbContext.Illustrations.FindAsync(illustrationId) is null)
            return NotFound("no such illustration");
        if (await dbContext.Users.FindAsync(userId) is null)
            return NotFound("no such user");
        var item = await dbContext.FavoriteList.FindAsync(userId, illustrationId);
        if (favorite)
        {
            if (item is null)
                await dbContext.FavoriteList.AddAsync(new()
                {
                    DateTime = DateTime.UtcNow,
                    IllustrationId = illustrationId,
                    UserId = userId,
                });
        }
        else
        {
            if (item is not null)
                dbContext.FavoriteList.Remove(item);
        }

        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
