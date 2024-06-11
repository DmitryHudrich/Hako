using Grpc.Core;
using Server.ServerGate;

namespace Server.ServerGate.Services;

public class GreeterService(ILogger<GreeterService> logger) : Greeter.GreeterBase {
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        return Task.FromResult(new HelloReply {
            Message = "Hello " + request.Name
        });
    }
}
