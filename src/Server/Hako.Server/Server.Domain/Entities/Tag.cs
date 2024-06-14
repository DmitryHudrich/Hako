namespace Server.Domain.Entities;

public class Tag : IEntity {
    public Int64 Id { get; set; }
    public required String Name { get; set; }
    public required String Description { get; set; }
}
