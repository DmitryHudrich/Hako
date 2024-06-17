namespace Server.Domain.Entities;

public class HakoFileAccess : IEntity<Guid> {
    public Guid Id { get; set; }
    public ReadVisibility ReadRights { get; set; }
    public WriteVisibility WriteVisibility { get; set; }
}
