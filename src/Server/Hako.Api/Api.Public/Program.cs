using System.Threading.Tasks;
using Grpc.Net.Client;
using Api.ServiceClient;
using Server.ServerGate;
using Microsoft.AspNetCore.Authorization;


var a = new Class1();
await a.BebraAsync();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/file", [Authorize]
async (String internalPath) => {
    using var channel = GrpcChannel.ForAddress("https://localhost:1488");
    var client = new HakoFileService.HakoFileServiceClient(channel);
    var reply = await client.GetFileAsync(new GetFileRequest { InternalPath = internalPath });
});

app.Run();

record WeatherForecast(DateOnly Date, Int32 TemperatureC, String? Summary) {
    public Int32 TemperatureF => 32 + (Int32)(TemperatureC / 0.5556);
}
