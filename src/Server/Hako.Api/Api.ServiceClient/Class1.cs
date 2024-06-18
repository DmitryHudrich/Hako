namespace Api.ServiceClient;

public class Class1
{
    public async Task BebraAsync()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5034");
        var client = new Greeter.GreeterClient(channel);

        var reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" });
        Console.WriteLine("Greeting: " + reply.Message);

        Console.WriteLine("Shutting down");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
