using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController(ILogger<CommentController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("list")]
    public IQueryable<CommentItem> List(long illustrationId)
    {
        return dbContext.CommentList.Where(t => t.IllustrationId == illustrationId)
            .OrderByDescending(t => t.DateTime)
            .Include(t => t.User);
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostAsync(long userId, long illustrationId, string comment)
    {
        if (await dbContext.Illustrations.FindAsync(illustrationId) is null)
            return NotFound("no such illustration");
        if (await dbContext.Users.FindAsync(userId) is null)
            return NotFound("no such user");
        _ = await dbContext.CommentList.AddAsync(new CommentItem()
        {
            IllustrationId = illustrationId,
            UserId = userId,
            DateTime = DateTime.UtcNow,
            Comment = comment
        });
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> PostAsync(long userId, long illustrationId, DateTime time)
    {
        var item = await dbContext.CommentList.FindAsync(userId, illustrationId, time);
        if (item is null)
            return NotFound("no such comment");
        _ = dbContext.CommentList.Remove(item);
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
