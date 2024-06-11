namespace Server.Domain.Entities;

public record class File(UInt64 Id, String Name, String Description, DateTime Creation);
public record class User(UInt64 Id, String Name, String Login, String Password, String Description, DateTime Registration);
public record class Tag(UInt64 Id, String Name, String Description) : INamedHakoEntity;
