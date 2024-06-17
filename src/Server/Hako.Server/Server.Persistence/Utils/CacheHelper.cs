using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;

namespace Server.Persistence.Utils;

public class CacheHelper<TEntity, TKey>(ILogger<CacheHelper<TEntity, TKey>> logger, IDistributedCache cache) 
    where TKey : notnull
    where TEntity : class, IEntity<TKey> {
    /// <summary>
    /// Check cache for <typeparamref name="TEntity"/> with <paramref name="id"/>.
    /// If cache miss, execute <paramref name="dbAction"/>, return his result and save him to cache.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the cache key.</typeparam>
    /// <param name="id">Cache key.</param>
    /// <param name="dbAction">The action to call if the entity is not found in the cache.</param>
    public async Task<TEntity?> CheckCacheAsync(TKey id, Func<ValueTask<TEntity?>> dbAction) {
        var jsonEntity = await cache.GetStringAsync(id.GetHashCode().ToString());
        return jsonEntity is null ? await CacheMissAsync(dbAction) : CacheHit(jsonEntity);
    }

    private TEntity CacheHit(String jsonEntity) {
        logger.LogDebug("Cache hit");
        var entity = JsonSerializer.Deserialize<TEntity>(jsonEntity);
        ArgumentNullException.ThrowIfNull(entity, nameof(entity) + " [" + jsonEntity + "] Broken cache");
        return entity;
    }

    private async Task<TEntity?> CacheMissAsync(Func<ValueTask<TEntity?>> dbAction) {
        logger.LogDebug("Cache miss");
        var entity = await dbAction();
        if (entity is null) {
            return default;
        }
        await cache.SetStringAsync(entity.Id.GetHashCode().ToString(), JsonSerializer.Serialize(entity));
        return entity;
    }
}