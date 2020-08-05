using System;
using System.Reflection;

namespace Bannerlord.ButterLib.CampaignIdentifier.Helpers
{
    internal static class DebugHelper
    {
        public static void HandleException(Exception ex, MethodBase methodBase, string sectionName)
        {
            LoggingHelper.Log($"Error occured{(methodBase != null ? $" in {methodBase}" : "")} - {ex}", sectionName);
        }

        public static void HandleException(Exception ex, string sectionName, string logMessage)
        {
            LoggingHelper.Log(string.Format(logMessage, ex), sectionName);
        }

        public static ArgumentOutOfRangeException GetOutOfRangeException<T>(T value, string functionName, string argumentName)
        {
            return new ArgumentOutOfRangeException(argumentName, value, $"{functionName} is supplied with not supported {typeof(T).Name} value.");
        }
    }
}