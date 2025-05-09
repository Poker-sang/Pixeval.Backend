using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GetController(ILogger<NewWorksController> logger, PixevalDbContext dbContext) : Controller
{
    [HttpGet("illustration")]
    public async Task<IActionResult> IllustrationAsync(long userId, long illustrationId)
    {
        var illustration = await dbContext.Illustrations
            .Include(t => t.User)
            .FirstOrDefaultAsync(i => i.Id == illustrationId);
        if (illustration is null)
            return NotFound("no such illustration");
        illustration.IsFavorite = await dbContext.FavoriteList.FindAsync(userId, illustrationId) is not null;
        return Ok(illustration);
    }

    [HttpGet("user")]
    public async Task<IActionResult> UserAsync(long userId, long followedUserId)
    {
        var user = await dbContext.Users.FindAsync(followedUserId);
        if (user is null)
            return NotFound("no such illustration");
        user.IsFollowed = await dbContext.FollowList.FindAsync(userId, followedUserId) is not null;
        return Ok(user);
    }
}
