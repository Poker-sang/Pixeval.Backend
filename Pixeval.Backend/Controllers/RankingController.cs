using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;
using Pixeval.Backend.Services;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RankingController(ILogger<RankingController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Illustration>> List(long userId)
    {
        return await dbContext.Illustrations
            .OrderByDescending(t => t.TotalView)
            .Take(100)
            .Include(t => t.User)
            .SetFavoriteAsync(dbContext.FavoriteList, userId);
    }

    public class Foo
    {
        public double Total { get; set; }

        public double Avg { get; set; }

        public int Count { get; set; }

        public float Min { get; set; } = 1;

        public float Max { get; set; }
    }
}
