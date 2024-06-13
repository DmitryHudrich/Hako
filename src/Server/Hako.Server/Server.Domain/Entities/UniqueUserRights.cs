namespace Server.Domain.Entities;

public class UniqueUserRights {
    public Int64 Id { get; set; }
    public required User Consumer { get; set; }
    public required HakoFileAccess Rights { get; set; }
}
