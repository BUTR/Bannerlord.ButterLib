using System;
using System.Reflection;

namespace CampaignIdentifier.Helpers
{
  internal static class DebugHelper
  {
    public static void HandleException(Exception ex, MethodInfo methodInfo, string sectionName)
    {
      LoggingHelper.Log(string.Format("Error occured{0} - {1}", methodInfo != null ? $" in {methodInfo}" : "", ex.ToString()), sectionName);
    }

    public static void HandleException(Exception ex, string sectionName, string logMessage)
    {
      LoggingHelper.Log(string.Format(logMessage, ex.ToString()), sectionName);
    }
    public static ArgumentOutOfRangeException GetOutOfRangeException<T>(T value, string functionName, string argumentName)
    {
      return new ArgumentOutOfRangeException(argumentName, value, string.Format("{0} is supplied with not supported {1} value.", functionName, typeof(T).Name));
    }
  }
}
