using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;

namespace Server.Persistence;

public class HakoDbContext : DbContext {
    public DbSet<User> Users { get; set; } = null!;
}
