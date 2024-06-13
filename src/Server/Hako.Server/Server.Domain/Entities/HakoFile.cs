namespace Server.Domain.Entities;

public class HakoFile {
    public Int64 Id { get; set; }
    public required String PublicName { get; set; }
    public required String Description { get; set; }
    public required String Where { get; set; }
    public required HakoFileAccess Access { get; set; }
    public required DateTime Creation { get; set; }
    public required HakoFolder ContainedFolder { get; set; }
    public required List<UniqueUserRights> UniqueUserRights { get; set; } = [];
    public required List<Tag> Tags { get; set; } = [];
}
