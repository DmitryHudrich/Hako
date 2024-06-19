using Microsoft.EntityFrameworkCore;
using Server.Logic;
using Server.Persistence;

namespace Server.ServerGate.Internal;

internal static class ServerPreConfigurator {
    public static String ConnectionString => Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? throw new DatabaseConnectionNotSetException();

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
        services.AddScoped<UserLogicService>();
    }
}
