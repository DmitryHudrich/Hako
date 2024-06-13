using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server.Domain.Entities;
using Server.Persistence.Repos;

namespace Server.ServerGate.Services;

public class ServerService(ILogger<ServerService> logger, UserRepository repository) : Server.ServerBase {
    public override async Task<HelloReply> TestFeature(Empty request, ServerCallContext context) {

        var id = await repository.AddUserAsync(new User {
            Name = "Berba",
            Login = "asdsad",
            Password = "dasaadasd",
            Description = "askdjlasjflsadfasasdkfhalks",
            Registration = DateTime.UtcNow,
        });
        return new HelloReply {
            Message = id.ToString(),
        };
    }
}
