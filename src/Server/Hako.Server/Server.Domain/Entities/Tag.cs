namespace Server.Domain.Entities;

public class Tag {
    public Int64 Id { get; set; }
    public required String Name { get; set; }
    public required String Description { get; set; }
}
