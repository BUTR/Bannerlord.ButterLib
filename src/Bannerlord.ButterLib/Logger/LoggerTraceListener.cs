using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace System.Diagnostics.Logger
{
    internal class LoggerTraceListener : TraceListener
    {
        public override bool IsThreadSafe => true;
        public override string Name { get; set; } = "ILogger Tracer";

        private readonly ILogger _logger;

        public LoggerTraceListener(ILogger logger)
        {
            _logger = logger;
        }

        public override void Write(string message)
        {
            _logger.LogInformation(message);
        }

        public override void WriteLine(string message)
        {
            _logger.LogInformation(message);
        }

        public override void Fail(string message)
        {
            base.Fail(message);
        }

        public override void Fail(string message, string detailMessage)
        {
            base.Fail(message, detailMessage);

        }
    }
}