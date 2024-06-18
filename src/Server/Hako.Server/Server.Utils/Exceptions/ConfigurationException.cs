namespace Server.Utils.Exceptions;

[Serializable]
internal class ConfigurationException : Exception {
    private String description;
    private String commandLineArgumentName;
    private String environmentVariableName;

    public ConfigurationException() {
    }

    public ConfigurationException(String? message) : base(message) {
    }

    public ConfigurationException(String? message, Exception? innerException) : base(message, innerException) {
    }

    public ConfigurationException(String description, String commandLineArgumentName, String environmentVariableName) {
        this.description = description;
        this.commandLineArgumentName = commandLineArgumentName;
        this.environmentVariableName = environmentVariableName;
    }
}


/* TODO: Adding more configuration properties */

// internal record ManyValuesConfigurationProperty<T>(
//     String CommandLineArgumentName,
//     String JsonPropertyName,
//     String EnvironmentVariableName,
//     String Description,
//     Dictionary<Regex, LoadConfigurationHandler<T>>? AvailableValues,
//     String? DefaultValue = null,
//     Boolean IsRequired = false);
