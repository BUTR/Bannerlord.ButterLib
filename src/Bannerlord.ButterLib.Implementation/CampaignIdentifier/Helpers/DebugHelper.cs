using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers
{
    internal static class DebugHelper
    {
        public static void HandleException(Exception ex, MethodBase methodBase, string sectionName)
        {
            SubModule.Logger.LogError(exception: ex, message: "{sectionName} - Error occured in {methodBase}.", sectionName, methodBase.Name);
        }

        public static void HandleException(Exception ex, string logMessage)
        {
            SubModule.Logger.LogError(ex, logMessage);
        }

        public static ArgumentOutOfRangeException GetOutOfRangeException<T>(T value, string functionName, string argumentName)
        {
            return new ArgumentOutOfRangeException(argumentName, value, $"{functionName} is supplied with not supported {typeof(T).Name} value.");
        }
    }
}