namespace Server.Domain.Entities;

public class User {
    public Int64 Id { get; set; }
    public required String Name { get; set; }
    public required String Login { get; set; }
    public required String Password { get; set; }
    public required String Description { get; set; }
    public required DateTime Registration { get; set; }
}
