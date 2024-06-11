namespace Server.Domain.Entities;

public interface INamedHakoEntity : IHakoEntity {
    String Name { get; }
    String Description { get; }
}

