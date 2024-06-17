using Microsoft.EntityFrameworkCore;
using Server.Persistence;
using Server.Persistence.Repos;
using Server.Persistence.Utils;

namespace Server.ServerGate.Internal;

internal static class PreConfigurator {
    public static void Configure(WebApplicationBuilder builder) {
        ConfigureServices(builder.Services);
        if (Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") == null) {
            builder.WebHost.UseUrls("http://127.0.0.1:1488");
        }
    }

    private static void ConfigureServices(IServiceCollection services) {
        // Add services to the container.
        services.AddGrpc();
        services.AddDbContext<HakoDbContext>(options =>
            options.UseNpgsql("""
                Host=database_pg;
                Database=HakoDb;
                Username=postgres;
                Password=postgres;
            """));
        services.AddStackExchangeRedisCache(options => {
            options.Configuration = "database_redis";
            options.InstanceName = "local";
        });
        services.AddScoped(typeof(CacheHelper<,>));
        services.AddScoped<UserRepository>();
    }
}