using Microsoft.AspNetCore.Mvc;
using Pixeval.Backend.Models;
using Pixeval.Backend.Services;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FollowController(ILogger<FavoriteController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IEnumerable<User>> ListAsync(long userId)
    {
        return await dbContext.FollowList
            .Where(t => t.UserId == userId)
            .Select(t => t.FollowedUser)
            .SelfForEachAsync(t => t.IsFollowed = true);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(long userId, long followedUserId, bool follow)
    {
        if (await dbContext.Users.FindAsync(followedUserId) is null || await dbContext.Users.FindAsync(userId) is null)
            return NotFound("no such user");
        var item = await dbContext.FollowList.FindAsync(userId, followedUserId);
        if (follow)
        {
            if (item is null)
                await dbContext.FollowList.AddAsync(new()
                {
                    FollowedUserId = followedUserId,
                    UserId = userId,
                });
        }
        else
        {
            if (item is not null)
                dbContext.FollowList.Remove(item);
        }

        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
