namespace Server.Utils.Exceptions;

[Serializable]
internal class ConfigurationNotSetException : Exception {
    public ConfigurationNotSetException() {
    }

    public ConfigurationNotSetException(String? message) : base(message) {
    }

    public ConfigurationNotSetException(String? message, Exception? innerException) : base(message, innerException) {
    }
}
