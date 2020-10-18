using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Logger.Extensions
{
    public static class LoggerExtensions
    {
        private static string MessageFormatter(object state, Exception error) => state.ToString();

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
                LogLevel.Warning => Color.FromUint(0x00FFFF00), // yellow
                LogLevel.Error => Color.FromUint(0x00FF8000), // orange
                LogLevel.Critical => Color.FromUint(0x00FF0000), // red
                LogLevel.None => Color.White,
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
            }));
        }
    }
}
