using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;
using Pixeval.Backend.Services;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class NewWorksController(ILogger<NewWorksController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Illustration>> ListAsync(long userId)
    {
        return await dbContext.FollowList.Where(t => t.UserId == userId)
            .SelectMany(t => t.FollowedUser.Posts)
            .OrderByDescending(t => t.CreateDate)
            .Take(100)
            .Include(t => t.User)
            .SetFavoriteAsync(dbContext.FavoriteList, userId);
    }

    [HttpGet("all")]
    public async Task<IEnumerable<Illustration>> ListAllAsync(long userId)
    {
        return await dbContext.Illustrations
            .OrderByDescending(t => t.CreateDate)
            .Take(100)
            .Include(t => t.User)
            .SetFavoriteAsync(dbContext.FavoriteList, userId);
    }
}
