using Microsoft.EntityFrameworkCore;
using Server.Persistence;
using Server.Persistence.Repos;
using Server.Persistence.Utils;
using Server.ServerGate.Internal;
using Server.ServerGate.Services;

var builder = WebApplication.CreateBuilder(args);

PreConfigurator.Configure(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ServerService>();
app.MapGrpcService<GreeterService>();
app.MapGet("/", () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

