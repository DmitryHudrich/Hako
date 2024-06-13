namespace Server.Domain.Entities;

public class HakoFolder {
    public Int64 Id { get; set; }
    public required String Name { get; set; }
    public required String Description { get; set; }
    public required HakoFolder ContainedFolder { get; set; }
    public required List<HakoFile> Files { get; set; }
    public required List<UniqueUserRights> UniqueUserRights { get; set; } = [];
    public required List<HakoFolder> Folders { get; set; }
}
