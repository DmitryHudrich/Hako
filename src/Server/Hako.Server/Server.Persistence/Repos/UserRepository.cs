using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;

namespace Server.Persistence.Repos;

internal delegate Task<User?> FilterHandler<TParam>(TParam param);

public enum UserFilterType {
    ById,
    ByLogin,
}

public class UserRepository(HakoDbContext dbContext) {
    public async Task<User?> GetUserByFilterAsync<TFilter>(UserFilterType filterType, TFilter filter) {
        var res = filterType switch {
            UserFilterType.ById => HandleFilterAsync<UInt64>(ByIdAsync),
            UserFilterType.ByLogin => HandleFilterAsync<String>(ByLoginAsync),
            _ => throw new ArgumentException("Bebra"),
        };

        return await res;

        async Task<User?> HandleFilterAsync<TCompare>(FilterHandler<TCompare> handler) {
            return filter is TCompare f
                ? await handler(f)
                : throw new ArgumentException($"expected {nameof(TCompare)}, but {nameof(TFilter)}");
        }
    }

    public async Task<UInt64?> AddUserAsync(User user) {
        _ = dbContext.Users.Add(user);
        var entries = await dbContext.SaveChangesAsync();
        // return entries < 0 ? user.Id : null;
        return 0;
    }

    private async Task<User?> ByLoginAsync(String login) {
        return await dbContext.Users.FirstOrDefaultAsync(obj => obj.Login == login);
    }

    private async Task<User?> ByIdAsync(UInt64 id) {
        return await dbContext.Users.FindAsync(id);
    }
}
