using Grpc.Core;
using Server.Persistence.Repos;

namespace Server.ServerGate.Services;

public class GreeterService(ILogger<GreeterService> logger, UserRepository repository) : Greeter.GreeterBase {
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
        logger.LogDebug((repository.GetUserByFilterAsync(UserFilterType.ById, 0) is null).ToString());
        return Task.FromResult(new HelloReply {
            Message = "Hello " + request.Name
        });
    }
}
