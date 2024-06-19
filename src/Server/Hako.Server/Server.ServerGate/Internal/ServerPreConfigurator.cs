using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.Logic;
using Server.Logic.Internal;
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
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    // указывает, будет ли валидироваться издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представляющая издателя
                    ValidIssuer = JwtData.Issuer,
                    // будет ли валидироваться потребитель токена
                    ValidateAudience = true,
                    // установка потребителя токена
                    ValidAudience = JwtData.Audience,
                    // будет ли валидироваться время существования
                    ValidateLifetime = true,
                    // установка ключа безопасности
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_DUPER_SECRET_KEY")),
                    // валидация ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });
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
    }
}
