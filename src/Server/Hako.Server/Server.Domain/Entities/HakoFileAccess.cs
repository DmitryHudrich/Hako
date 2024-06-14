namespace Server.Domain.Entities;

public class HakoFileAccess : IEntity {
    public Int64 Id { get; set; }
    public ReadVisibility ReadRights { get; set; }
    public WriteVisibility WriteVisibility { get; set; }
}
