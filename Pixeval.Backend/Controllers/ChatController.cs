using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(ILogger<ChatController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("list")]
    public IQueryable<ChatItem> List(string tag)
    {
        return dbContext.ChatList.Where(t => t.Tag == tag)
            .OrderByDescending(t => t.DateTime)
            .Include(t => t.User);
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostAsync(long userId, string tag, string text)
    {
        if (await dbContext.Users.FindAsync(userId) is null)
            return NotFound("no such user");
        _ = await dbContext.ChatList.AddAsync(new ChatItem()
        {
            Tag = tag,
            UserId = userId,
            DateTime = DateTime.UtcNow,
            Text = text
        });
        await dbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("delete")]
    public async Task<IActionResult> PostAsync(long userId, string tag, DateTime time)
    {
        var item = await dbContext.ChatList.FindAsync(userId, tag, time);
        if (item is null)
            return NotFound("no such chat");
        _ = dbContext.ChatList.Remove(item);
        await dbContext.SaveChangesAsync();
        return Ok();
    }
}
