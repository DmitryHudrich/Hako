namespace Server.ServerGate.Internal;

[Serializable]
internal class DatabaseConnectionNotSetException : Exception {
    public DatabaseConnectionNotSetException() {
    }

    public DatabaseConnectionNotSetException(String? message) : base(message) {
    }

    public DatabaseConnectionNotSetException(String? message, Exception? innerException) : base(message, innerException) {
    }
}
