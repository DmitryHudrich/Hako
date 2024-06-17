namespace Server.Domain.Entities;

public class UniqueUserRights : IEntity<Guid> {
    public Guid Id { get; set; }
    public required User Consumer { get; set; }
    public required HakoFileAccess Rights { get; set; }
}
