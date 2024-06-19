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
        dbContext.HakoFiles.Add(file);
        await dbContext.SaveChangesAsync();
        return new AddFileResponse {
            Message = "File added successfully.",
            Success = true,
            Detail = new FileInfo { InternalPath = internalName }
        };
    }

    [Authorize]
    public override async Task<DeleteFileResponse> DeleteFile(DeleteFileRequest request, ServerCallContext context) {
        var fileinfo = await dbContext.HakoFiles.FirstOrDefaultAsync(file => file.Path == request.InternalPath);
        if (fileinfo == null) {
            return new DeleteFileResponse {
                Message = "File not found.",
                Success = false,
                Detail = new FileDeleteInfo { IsFileFound = false }
            };
        }
        await Task.Run(() => File.Delete(fileinfo.Path));

        dbContext.HakoFiles.Remove(fileinfo);
        await dbContext.SaveChangesAsync();
        return new DeleteFileResponse {
            Message = "File deleted successfully.",
            Success = true,
            Detail = new FileDeleteInfo { IsFileFound = true }
        };
    }

    [Authorize]
    public override async Task<GetFileSignaturesResponse> GetFileSigntatures(GetFileSignaturesRequest request, ServerCallContext context) {
        var files = await dbContext.HakoFiles.ToListAsync();

        var response = new GetFileSignaturesResponse { Message = "Success", Success = true, Detail = new GetFileSignaturesInfo() };
        foreach (var file in files) {
            response.Detail.FileSignatureInfo.Add(new FileSignatureInfo {
                InternalPath = file.Path,
                PublicName = file.PublicName,
                Description = file.Description,
            });
        }

        return response;
    }
}
