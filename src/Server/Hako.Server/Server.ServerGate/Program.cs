using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Server.Persistence;
using Server.Persistence.Repos;
using Server.Persistence.Utils;
using Server.ServerGate;
using Server.ServerGate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<HakoDbContext>(options =>
    options.UseNpgsql("Host=172.17.0.2;Database=HakoDb;Username=postgres;Password=postgresmaster;"));
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "172.17.0.4";
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

