namespace Server.Utils;

internal delegate TReturn LoadConfigurationHandler<TReturn>(String parsedValue);

// internal static class Configuration {
//     public static String DatabaseConnectionString => LoadConfiguration(
//         new ConfigurationProperty<String>(
//             CommandLineArgumentName: "database-connection-string",
//             JsonPropertyName: "databaseConnectionString",
//             EnvironmentVariableName: "DATABASE_CONNECTION_STRING",
//             Description: "Database connection string",
//             AvailableValue:
//                 (
//                     new Regex(@"Host=(?<host>[^;]+);Port=(?<port>\d+);Database=(?<database>[^;]+);(Username|User Id)=(?<username>[^;]+);Password=(?<password>[^;]+)",
//                         RegexOptions.IgnoreCase),
//                     static st => st
//                 ),
//             IsRequired: true)
//     );
//     public static String ServerHost { get => serverHost; private set => serverHost = value; }
//     public static Int32 ServerPort { get => serverPort; private set => serverPort = value; }


//     /* todo:
//      * Checking cli argument
//      * Json property
//      */
//     private static TProperty LoadConfiguration<TProperty>(ConfigurationProperty<TProperty> configurationProperty) {
//         var environmentVariable = Environment.GetEnvironmentVariable(configurationProperty.EnvironmentVariableName);
//         var commandLineArgument = Environment.GetCommandLineArgs().FirstOrDefault(x => x.StartsWith(configurationProperty.CommandLineArgumentName)) + 1;
//         /* todo: json property */
//         String? jsonProperty = null;

//         if (jsonProperty != null) {
//             return configurationProperty.AvailableValue.handler(jsonProperty);
//         }

//         if (commandLineArgument != null) {
//             return configurationProperty.AvailableValue.handler(commandLineArgument);
//         }

//         if (environmentVariable != null) {
//             return configurationProperty.AvailableValue.handler(environmentVariable);
//         }

//         var defaultValue = configurationProperty.DefaultValue;

//         if (defaultValue != null) {
//             return configurationProperty.AvailableValue.handler(defaultValue);
//         }

//         throw new ConfigurationException(configurationProperty.Description, configurationProperty.CommandLineArgumentName, configurationProperty.EnvironmentVariableName);
//     }

//     public static void HandleConfiguration() {

//     }
// }

// internal record ConfigurationProperty<T>(
//     String CommandLineArgumentName,
//     String JsonPropertyName,
//     String EnvironmentVariableName,
//     String Description,
//     (Regex regex, LoadConfigurationHandler<T> handler) AvailableValue,
//     String? DefaultValue = null,
//     Boolean IsRequired = false);


// /* TODO: Adding more configuration properties */

// // internal record ManyValuesConfigurationProperty<T>(
// //     String CommandLineArgumentName,
// //     String JsonPropertyName,
// //     String EnvironmentVariableName,
// //     String Description,
// //     Dictionary<Regex, LoadConfigurationHandler<T>>? AvailableValues,
// //     String? DefaultValue = null,
// //     Boolean IsRequired = false);
