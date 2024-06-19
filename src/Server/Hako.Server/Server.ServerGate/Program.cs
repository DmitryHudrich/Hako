using Server.ServerGate;
using Server.ServerGate.Internal;
using Server.ServerGate.Services;

var builder = WebApplication.CreateBuilder(args);

ServerPreConfigurator.Configure(builder);

var app = builder.Build();

if (Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") != null) {
    app.Logger.LogInformation("Running in container");
}

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<AuthService>();
app.MapGrpcService<FileService>();
app.MapGet("/", () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

