namespace Server.Domain.Entities;

public enum ReadVisibility {
    All,
    RefOwners,
    Nobody,
}

public enum WriteVisibility {
    All,
    RefOwners,
    Nobody,
}
