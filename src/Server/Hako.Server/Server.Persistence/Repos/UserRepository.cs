using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;
using Server.Persistence.Utils;

namespace Server.Persistence.Repos;

internal delegate Task<TEntity?> FilterHandler<TKey, TEntity>(TKey param);

public enum UserFilterType {
    ById,
    ByLogin,
}

public class UserRepository(HakoDbContext dbContext, ILogger<UserRepository> logger, CacheHelper cacheHelper) {
    public async Task<User?> GetUserByFilterAsync<TFilter>(UserFilterType filterType, TFilter filter) {
        var res = filterType switch {
            UserFilterType.ById => HandleFilterAsync<Int64>(ByIdAsync),
            UserFilterType.ByLogin => HandleFilterAsync<String>(ByLoginAsync),
            _ => throw new ArgumentException("Bebra"),
        };

        return await res;

        async Task<User?> HandleFilterAsync<TKey>(FilterHandler<TKey, User> handler) {
            return filter is TKey f
                ? await handler(f)
                : throw new ArgumentException($"expected {nameof(TKey)}, but {nameof(TFilter)}");
        }
    }

    public async Task<Int64> AddUserAsync(User user) {
        _ = dbContext.Users.Add(user);
        var entries = await dbContext.SaveChangesAsync();
        return entries > 0 ? user.Id : -1;
    }

    private async Task<User?> ByLoginAsync(String login) {
        return await dbContext.Users.FirstOrDefaultAsync(obj => obj.Login == login);
    }

    private async Task<User?> ByIdAsync(Int64 id) {
        return await cacheHelper.CheckCacheAsync(id, () => dbContext.Users.FindAsync(id));
    }
}
