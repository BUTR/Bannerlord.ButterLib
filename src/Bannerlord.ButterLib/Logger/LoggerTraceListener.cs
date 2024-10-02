using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.Logger;

internal class LoggerTraceListener : TraceListener
{
    private record ParseResult
    {
        public string Process { get; init; } = default!;
        public TraceEventType Level { get; init; } = default!;
        public int EventId { get; init; } = default!;
        public string Message { get; init; } = default!;
    }

    // There are some cases when System.Numerics.Vectors is not found
    // BLSE fixes that, but we might revert to string handling before BLSE is mandatory
    private static ParseResult? Parse(ReadOnlySpan<char> str)
    {
        try
        {
            if (str.IndexOf(':') is var logLevelIdx && logLevelIdx == -1)
                return null;

            var sourceLogLevel = str.Slice(0, logLevelIdx);
            var eventIdMessage = str.Slice(logLevelIdx + 1);

            if (sourceLogLevel.LastIndexOf(' ') is var sourceIdx && sourceIdx == -1)
                return null;
            var process = sourceLogLevel.Slice(0, sourceIdx);
            var logLevelStr = sourceLogLevel.Slice(sourceIdx + 1);

            if (!Enum.TryParse<TraceEventType>(logLevelStr.ToString(), out var logLevel))
                return null;

            var eventIdIdx = eventIdMessage.IndexOf(':');
            if (eventIdIdx == -1)
                return null;
            if (!int.TryParse(eventIdMessage.Slice(0, eventIdIdx).ToString(), out var eventId))
                return null;
            var message = eventIdMessage.Slice(eventIdIdx + 2);

            return new ParseResult
            {
                Process = process.ToString(),
                Level = logLevel,
                EventId = eventId,
                Message = message.ToString()
            };
        }
        catch (Exception e)
        {
            return new ParseResult
            {
                Process = "UNKNOWN",
                Level = TraceEventType.Error,
                EventId = 0,
                Message = $"Failed to parse log message: {str.ToString()}! Please report this issue to the ButterLib developers! Exception: {e}"
            };
        }
    }


    public override bool IsThreadSafe => true;
    public override string Name { get; set; } = "ILogger Tracer";

    private readonly ILogger _logger;

    public LoggerTraceListener(ILogger logger)
    {
        _logger = logger;
    }

    private void Log(ParseResult result)
    {
        switch (result.Level)
        {
            case TraceEventType.Critical:
                _logger.LogCritical("{Message}", result.Message);
                break;
            case TraceEventType.Error:
                _logger.LogError("{Message}", result.Message);
                break;
            case TraceEventType.Warning:
                _logger.LogWarning("{Message}", result.Message);
                break;
            case TraceEventType.Information:
                _logger.LogInformation("{Message}", result.Message);
                break;
            case TraceEventType.Verbose:
                _logger.LogTrace("{Message}", result.Message);
                break;

            case TraceEventType.Start:
            case TraceEventType.Stop:
            case TraceEventType.Suspend:
            case TraceEventType.Resume:
            case TraceEventType.Transfer:
            default:
                break;
        }
    }

    public override void Write(string? message)
    {
        if (Parse(message.AsSpan()) is { } result)
            Log(result);
        else
            _logger.LogInformation("{Message}", message);
    }

    public override void WriteLine(string? message)
    {
        if (Parse(message.AsSpan()) is { } result)
            Log(result);
        else
            _logger.LogInformation("{Message}", message);
    }
}