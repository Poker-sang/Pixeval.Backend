using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pixeval.Backend.Models;

namespace Pixeval.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController(ILogger<RecommendationController> logger, PixevalDbContext dbContext) : ControllerBase
{
    [HttpGet("illustrations")]
    public async Task<IEnumerable<Illustration>> ListIllustrationAsync(long userId)
    {
        var (favoriteItems, mainTags) = await GetAsync(userId);

        List<(double Sim, Illustration Illustration)> simList = [];

        foreach (var illustration in dbContext.Illustrations.Include(t => t.User))
        {
            if (await favoriteItems.AnyAsync(t => t.IllustrationId == illustration.Id))
                continue;

            illustration.IsFavorite = false;

            var sim = 0d;
            foreach (var (key, value) in illustration.Tags3.GeneralRes)
                if (mainTags.TryGetValue(key, out var v))
                    sim += value * v.Avg;

            simList.Add((sim, illustration));
        }

        simList.Sort((tuple1, tuple2) => - tuple1.Sim.CompareTo(tuple2.Sim));

        return simList.Select(t => t.Illustration).Take(100);
    }

    [HttpGet("users")]
    public async Task<IEnumerable<User>> ListUserAsync(long userId)
    {
        var (_, mainTags) = await GetAsync(userId);

        List<(double Sim, Illustration Illustration)> simList = [];

        foreach (var illustration in dbContext.Illustrations.Include(t => t.User))
        {
            var sim = 0d;
            foreach (var (key, value) in illustration.Tags3.GeneralRes)
                if (mainTags.TryGetValue(key, out var v))
                    sim += value * v.Avg;

            simList.Add((sim, illustration));
        }

        simList.Sort((tuple1, tuple2) => - tuple1.Sim.CompareTo(tuple2.Sim));

        var userList = new HashSet<User>(100);
        foreach (var illustration in simList)
        {
            var user = illustration.Illustration.User; 
            if (await dbContext.FollowList.FindAsync(userId, user.Id) is not null)
                continue;
            if (userList.Add(new User(illustration.Illustration.User)))
                if (userList.Count >= 100)
                    break;
        }

        return userList;
    }

    private async Task<(IIncludableQueryable<FavoriteItem, Illustration> FavoriteItems, Dictionary<string, Foo> MainTags)> GetAsync(long userId)
    {
        var favoriteItems = dbContext.FavoriteList.Where(t => t.UserId == userId)
            .Include(favoriteItem => favoriteItem.Illustration);

        var dict = new Dictionary<string, Foo>();

        foreach (var id in favoriteItems)
        {
            foreach (var general in id.Illustration.Tags3.GeneralRes)
            {
                if (!dict.TryGetValue(general.Key, out var foo))
                    foo = dict[general.Key] = new Foo();

                foo.Count++;
                foo.Total += general.Value;
                foo.Min = MathF.Min(foo.Min, general.Value);
                foo.Max = MathF.Max(foo.Max, general.Value);
            }
        }

        var mainTags = new Dictionary<string, Foo>();
        var count = await favoriteItems.CountAsync();
        foreach (var foo in dict.Where(foo => foo.Value.Count > 1))
        {
            var value = mainTags[foo.Key] = foo.Value;
            value.Avg = value.Total / count;
        }

        return (favoriteItems, mainTags);
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
