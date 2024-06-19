using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;

namespace Server.Persistence;

public class HakoDbContext : DbContext {
    public HakoDbContext(DbContextOptions<HakoDbContext> options)
       : base(options) {
        // _ = Database.EnsureDeleted();
        _ = Database.EnsureCreated();   // создаем базу данных при первом обращении
    }

    public DbSet<User> Users { get; set; }
    public DbSet<HakoFile> HakoFiles { get; set; }
}