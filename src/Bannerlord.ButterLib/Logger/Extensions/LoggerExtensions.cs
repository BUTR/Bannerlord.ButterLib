using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

using System;

using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Logger.Extensions
{
    public static class LoggerExtensions
    {
        private static string MessageFormatter(object state, Exception error) => state.ToString() ?? string.Empty;

        public static void LogAndDisplay(this ILogger logger, LogLevel logLevel, string message, params object[] args) =>
            LogAndDisplay(logger, logLevel, null!, message, args );

        public static void LogAndDisplay(this ILogger logger, LogLevel logLevel, Exception exception, string message, params object[] args)
        {
            var formattedValues = new FormattedLogValues(message, args);
            logger.Log(logLevel, 0, formattedValues, exception, MessageFormatter);
            InformationManager.DisplayMessage(new InformationMessage($"{logLevel}: {formattedValues}", logLevel switch
            {
                LogLevel.Trace => Color.FromUint(0x00FFFFFF), // white
                LogLevel.Debug => Color.FromUint(0x00808080), // grey
                LogLevel.Information => Color.FromUint(0x00008000), // green
                LogLevel.Warning => Color.FromUint(0x00FF8000), // orange
                LogLevel.Error => Color.FromUint(0x00FF0000), // red
                LogLevel.Critical => Color.FromUint(0x008B0000), // dark red
                LogLevel.None => Color.White,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            }));
        }

        public static void LogTraceAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Trace, null!, message, args);
        public static void LogDebugAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Debug, null!, message, args);
        public static void LogInformationAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Information, null!, message, args);
        public static void LogWarningAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Warning, null!, message, args);
        public static void LogErrorAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Error, null!, message, args);
        public static void LogCriticalAndDisplay(this ILogger logger, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Critical, null!, message, args);

        public static void LogTraceAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Trace, exception, message, args);
        public static void LogDebugAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Debug, exception, message, args);
        public static void LogInformationAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Information, exception, message, args);
        public static void LogWarningAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Warning, exception, message, args);
        public static void LogErrorAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Error, exception, message, args);
        public static void LogCriticalAndDisplay(this ILogger logger, Exception exception, string message, params object[] args) =>
            LogAndDisplay(logger, LogLevel.Critical, exception, message, args);
    }
}
