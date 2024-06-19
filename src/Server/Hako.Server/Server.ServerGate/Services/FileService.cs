using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Server.Persistence;

namespace Server.ServerGate.Services;

public class FileService(ILogger<AuthService> logger, HakoDbContext dbContext) : HakoFileService.HakoFileServiceBase {
    [Authorize]
    public override async Task<AddFileResponse> AddFile(AddFileRequest request, ServerCallContext context) {
        throw new NotImplementedException();
    }
}
