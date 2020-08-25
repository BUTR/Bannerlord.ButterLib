By default, the [``ILogger``](xref:Microsoft.Extensions.Logging.ILogger) implementation will write it's logs in ``%GAME CONFIG%/ModLogs/default*.log``.  
You can create your own log file with this code:
```csharp
this.AddSerilogLoggerProvider("FILENAME.txt", new[] { FILTER });

// EXAMPLE:
this.AddSerilogLoggerProvider($"butterlib_{DateTimeOffset.Now:yyyyMMdd_HHmmss}.txt", new[] { "Bannerlord.ButterLib.*" });
```
Any log from a class from namespace ``Bannerlord.ButterLib.*`` will be written to the ``default*.log`` file and to your own file.  