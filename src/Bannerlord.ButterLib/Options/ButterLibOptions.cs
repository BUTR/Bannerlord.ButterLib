using Microsoft.Extensions.Logging;

namespace Bannerlord.ButterLib.Options;

public sealed class ButterLibOptions
{
    public int MinLogLevel { get; set; } = (int) LogLevel.Information;
}