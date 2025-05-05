using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend.Services;

public static class StaticContext
{
    public const string ContextDirectory = "D:/毕设";

    public static string GetContextFile(string file)
    {
        return Path.Combine(ContextDirectory, file);
    }

    public static FileStream OpenAsyncReadContext(string file, int bufferSize = 4096)
    {
        return OpenAsyncRead(GetContextFile(file), bufferSize);
    }
    
    public static FileStream OpenAsyncWriteContext(string file, int bufferSize = 4096)
    {
        return OpenAsyncWrite(GetContextFile(file), bufferSize);
    }
    
    public static FileStream CreateAsyncWriteContext(string file, int bufferSize = 4096)
    {
        return CreateAsyncWrite(GetContextFile(file), bufferSize);
    }

    public static FileStream OpenAsyncRead(string path, int bufferSize = 4096)
    {
        return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);
    }

    public static FileStream OpenAsyncWrite(string path, int bufferSize = 4096)
    {
        return new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, true);
    }

    public static FileStream CreateAsyncWrite(string path, int bufferSize = 4096)
    {
        return new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize, true);
    }

    public static async Task<IEnumerable<User>> SetFollowedAsync(this IEnumerable<User> users, DbSet<FollowItem> followItems, long myId)
    {
        foreach (var user in users) 
            user.IsFollowed = await followItems.FindAsync(myId, user.Id) is not null;
        return users;
    }

    public static async Task<IQueryable<T>> SelfForEachAsync<T>(this IQueryable<T> queryable, Action<T> action)
    {
        await queryable.ForEachAsync(action);
        return queryable;
    }

    public static async Task<IEnumerable<Illustration>> SetFavoriteAsync(this IQueryable<Illustration> illustrations, DbSet<FavoriteItem> favoriteItems, long myId)
    {
        foreach (var illustration in illustrations) 
            illustration.IsFavorite = await favoriteItems.FindAsync(myId, illustration.Id) is not null;
        return illustrations;
    }
}
