using Microsoft.EntityFrameworkCore;
using Server.Persistence;
using Server.ServerGate.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<HakoDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=HakoDb;"));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
// => >= <= ++ == 
app.Run();
