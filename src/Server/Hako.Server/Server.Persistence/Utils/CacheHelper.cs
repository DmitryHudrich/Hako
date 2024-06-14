using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Server.Domain.Entities;

namespace Server.Persistence.Utils;

public class CacheHelper(ILogger<CacheHelper> logger, IDistributedCache cache) {
    public async Task<TEntity?> CheckCacheAsync<TEntity, TKey>(TKey id, Func<ValueTask<TEntity?>> dbAction)
        where TEntity : class, IEntity
        where TKey : notnull {
        var jsonEntity = await cache.GetStringAsync(id.GetHashCode().ToString());
        return jsonEntity is null ? await CacheMissAsync(dbAction) : CacheHit<TEntity>(jsonEntity);
    }

    private TEntity CacheHit<TEntity>(String jsonEntity) {
        logger.LogDebug("Cache hit");
        var entity = JsonSerializer.Deserialize<TEntity>(jsonEntity);
        ArgumentNullException.ThrowIfNull(entity, nameof(entity) + " [" + jsonEntity + "] Broken cache");
        return entity;
    }

    private async Task<TEntity?> CacheMissAsync<TEntity>(Func<ValueTask<TEntity?>> dbAction) where TEntity : class, IEntity {
        logger.LogDebug("Cache miss");
        var entity = await dbAction();
        if (entity is null) {
            return null;
        }
        await cache.SetStringAsync(entity.Id.GetHashCode().ToString(), JsonSerializer.Serialize(entity));
        return entity;
    }
}