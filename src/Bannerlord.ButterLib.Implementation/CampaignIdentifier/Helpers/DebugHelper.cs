using System;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers
{
    internal static class DebugHelper
    {
        public static ArgumentOutOfRangeException GetOutOfRangeException<T>(T value, string functionName, string argumentName) =>
            new(argumentName, value, $"{functionName} is supplied with not supported {typeof(T).Name} value.");
    }
}