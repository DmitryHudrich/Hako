namespace Server.Domain.Entities;

public interface IEntity<TKey>
where TKey : notnull {
    TKey Id { get; }
}