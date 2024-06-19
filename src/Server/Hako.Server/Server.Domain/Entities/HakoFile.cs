namespace Server.Domain.Entities;

public class HakoFile : IEntity<Guid> {
    public Guid Id { get; set; }
    public required String PublicName { get; set; }
    public required String Path { get; set; }
    public required String Description { get; set; }
    public required DateTime Creation { get; set; }
    public required User Owner { get; set; }
}
