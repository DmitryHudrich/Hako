using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Server.Persistence;
using Server.Persistence.Repos;
using Server.Persistence.Utils;
using Server.ServerGate;
using Server.ServerGate.Services;

var builder = WebApplication.CreateBuilder(args);

if (Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") == null) {
    builder.WebHost.UseUrls("http://127.0.0.1:1488");
}

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<HakoDbContext>(options =>
    options.UseNpgsql("Host=database_pg;Database=HakoDb;Username=postgres;Password=postgres;"));
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "database_redis";
    options.InstanceName = "local";
});

builder.Services.AddScoped<CacheHelper>();
builder.Services.AddScoped<UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ServerService>();
app.MapGrpcService<GreeterService>();
app.MapGet("/", () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

