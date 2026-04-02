# PsdFramework.ModularWpf.Logging

#### Logging extensions for the `PsdFramework.ModularWpf` framework.

## How to configure?
It can only be configured through the `IConfiguration` interface, usually through the `appsettings.json` file.
The limitations are set for the sake of simplicity.
### Configuration example:
```json
{
  "ModularLoggers": [
    {
      "Sink": "File",
      "Path": "./logs/general.log",
      "MinimumLevel": "Information",
      "AllowedNamespaces": [ "ModularWpf" ]
    },
    {
      "Sink": "Console",
      "MinimumLevel": "Trace"
    }
  ]
}
```

#### If the allowed namespaces are not specified, the logger will allow everything.

#### Built-in namespace variables:
- `$RunningProjectName` - the name of the running project *(use-case: "AllowedNamespaces": [ "$RunningProjectName" ])*

## How to setup?
```csharp
serviceCollection.AddLogging(builder => {
    builder.AddPsdFramework(configuration); // configuration is IConfiguration instance
});
```
