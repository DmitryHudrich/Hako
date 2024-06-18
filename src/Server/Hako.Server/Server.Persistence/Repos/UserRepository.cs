using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;
using Server.Persistence.Repos.Tools;
using Server.Persistence.Utils;

namespace Server.Persistence.Repos;

internal delegate Task<TEntity> FilterHandler<TKey, TEntity>(TKey param);

public enum UserFilterType {
    ById,
    ByLogin,
    ByName,
}

public class UserRepository(HakoDbContext dbContext, ILogger<UserRepository> logger, CacheHelper<User, Int64> cacheHelper) :
#pragma warning disable CS9107 //It's ok
    EntityRepository<User, Int64>(dbContext, logger, cacheHelper) {
#pragma warning restore CS9107 
    /// <summary>
    /// Retrieves a user by applying a specific filter.
    /// </summary>
    /// <typeparam name="TFilter">The data type of the filter.</typeparam>
    /// <param name="filterType">The type of the filter.</param>
    /// <param name="filter">The filter value.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found, or null if not found.</returns>
    /// <exception cref="ArgumentException">Thrown if the filter type is not recognized.</exception>
    public async Task<IEnumerable<User?>> GetUserByFilterAsync<TFilter>(UserFilterType filterType, TFilter filter) {
        var res = filterType switch {
            UserFilterType.ById => HandleFilterAsync<Int64>(ByIdAsync),
            UserFilterType.ByLogin => HandleFilterAsync<String>(ByLoginAsync),
            UserFilterType.ByName => HandleFilterAsync<String>(ByNameAsync),
            _ => throw new ArgumentException("Bebra"),
        };

        return await res;

        async Task<IEnumerable<User?>> HandleFilterAsync<TKey>(FilterHandler<TKey, IEnumerable<User>> handler) {
            return filter is TKey f
                ? await handler(f)
                : throw new ArgumentException($"expected {nameof(TKey)}, but {nameof(TFilter)}");
        }
    }

    private async Task<IEnumerable<User>> ByNameAsync(String name) {
        var users = dbContext.Users.Where(obj => obj.Name == name);
        return await users.ToArrayAsync();
    }

    /// <summary>
    /// Adds a user to the database and returns the user's ID.
    /// </summary>
    /// <param name="user">The user to be added.</param>
    /// <returns>The ID of the added user, or -1 if the user was not added successfully.</returns>
    public async Task<Int64> AddAsync(User user) => await AddEntityAsync(user);

    public async Task<Boolean> DeleteAsync(User user) => await DeleteEntityAsync(user.Id);

    private async Task<IEnumerable<User>> ByLoginAsync(String login) {
        var user = await dbContext.Users.FirstOrDefaultAsync(obj => obj.Login == login);
        return user is null ? [] : [user];
    }
}
