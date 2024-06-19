using System.Security.Claims;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;
using Server.Persistence;

namespace Server.ServerGate.Services;

public class FileService(ILogger<AuthService> logger, HakoDbContext dbContext) : HakoFileService.HakoFileServiceBase {
    [Authorize]
    public override async Task<AddFileResponse> AddFile(AddFileRequest request, ServerCallContext context) {
        var internalName = Guid.NewGuid().ToString();
        using var fs = new FileStream(internalName, FileMode.Create, FileAccess.Write);
        await fs.WriteAsync(request.Content.ToByteArray());
        var file = new HakoFile {
            PublicName = request.Name,
            Path = internalName,
            Description = request.Description,
            Creation = DateTime.UtcNow,
            Owner = await dbContext.Users.FirstOrDefaultAsync(user =>
                user.Id == Int64.Parse(context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!)) ??
                    throw new Exception("User not found"),
        };
        return new AddFileResponse {
            Message = "File added successfully.",
            Success = true,
            Detail = new FileInfo { InternalPath = internalName }
        };
    }
}
