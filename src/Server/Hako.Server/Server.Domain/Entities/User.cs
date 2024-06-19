namespace Server.Domain.Entities;

public class User : IEntity<Int64> {
    public Int64 Id { get; set; }
    public required String Name { get; set; }
    public required String Email { get; set; }
    public required String Login { get; set; }
    public required String Password { get; set; }
    public required DateTime Registration { get; set; }
}
