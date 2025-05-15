using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;
using Pixeval.Backend.Services;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FollowController(ILogger<FavoriteController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("list")]
    public async Task<IQueryable<User>> ListAsync(long userId)
    {
        return await dbContext.FollowList.Where(t => t.UserId == userId)
            .Include(t => t.FollowedUser)
            .ThenInclude(t => t.Posts)
            .OrderByDescending(t => t.DateTime)
            .Select(t => t.FollowedUser)
            .Select(t => new User(t))
            .SelfForEachAsync(t => t.UserInfo.IsFollowed = true);
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
                    DateTime = DateTime.UtcNow,
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
