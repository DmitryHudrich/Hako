
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;
using Server.Persistence.Utils;

namespace Server.Persistence.Repos.Tools;

/// <summary>
/// Base class with some boilerplate code.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
abstract public class EntityRepository<TEntity, TKey>(HakoDbContext dbContext, ILogger<EntityRepository<TEntity, TKey>> logger, CacheHelper<TEntity, TKey> cacheHelper)
where TEntity : class, IEntity<TKey>
where TKey : notnull {
    /* 
     * todo docs
     */
    protected async Task<TKey?> AddEntityAsync(TEntity entity) {
        _ = dbContext.Set<TEntity>().Add(entity);
        var entries = await dbContext.SaveChangesAsync();
        return entries > 0 ? entity.Id : default;
    }

    protected async Task<TEntity?> ByIdAsync(TKey id) {
        return await cacheHelper.CheckCacheAsync(id, () => dbContext.Set<TEntity>().FindAsync(id));
    }

    protected async Task<Int64> DeleteEntitiesAsync(IEnumerable<TKey> entities) {
        var entitiesToDelete = dbContext.Set<TEntity>()
                                             .Where(e => entities.Contains(e.Id));
        dbContext.Set<TEntity>().RemoveRange(entitiesToDelete);
        var entries = await dbContext.SaveChangesAsync();
        return entries;
    }
    protected async Task<Boolean> DeleteEntityAsync(TKey id) => await DeleteEntitiesAsync([id]) > 0;
}