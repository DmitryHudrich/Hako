namespace Server.Persistence.Exceptions;

public class BrokenCacheException : Exception {
    public BrokenCacheException(String message) : base(message) {
    }
}