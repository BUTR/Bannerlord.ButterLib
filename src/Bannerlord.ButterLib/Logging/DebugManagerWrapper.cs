using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Logging
{
    /// <summary>
    /// 
    /// </summary>
    internal class DebugManagerWrapper : IDebugManager
    {
        private readonly ILogger _logger;
        private readonly IDebugManager _debugManager;

        public DebugManagerWrapper(IDebugManager debugManager)
        {
            _logger = SubModule.Services.BuildServiceProvider().GetRequiredService<ILogger<DebugManagerWrapper>>();
            _debugManager = debugManager;
        }

        public void ShowWarning(string message)
        {
            _logger.LogWarning(message);
            _debugManager.ShowWarning(message);
        }

        public void DisplayDebugMessage(string message)
        {
            _logger.LogDebug("Message: {message}}", message);
            _debugManager.DisplayDebugMessage(message);
        }

        public void Print(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416)
        {
            switch (logLevel)
            {
                case 1:
                    _logger.LogError("Message: {message}}", message);
                    break;
                case 2:
                    _logger.LogWarning("Message: {message}}", message);
                    break;
                case 3:
                    _logger.LogInformation("Message: {message}}", message);
                    break;
                case 4:
                    _logger.LogDebug("Message: {message}}", message);
                    break;
                default:
                    _logger.LogError("Message: {message}}", message);
                    break;
            }
            _debugManager.Print(message, logLevel, color, debugFilter);
        }
        public void PrintError(string error, string stackTrace, ulong debugFilter = 17592186044416)
        {
            _logger.LogError("Error: {error}; StackTrace: {stackTrace}", error, stackTrace);
            _debugManager.PrintError(error, stackTrace, debugFilter);
        }
        public void PrintWarning(string warning, ulong debugFilter = 17592186044416)
        {
            _logger.LogError("Warning: {warning}", warning);
            _debugManager.PrintWarning(warning, debugFilter);
        }

        public void SetCrashReportCustomString(string customString)
        {
            _logger.LogInformation("Crash Report: {customString}}", customString);
            _debugManager.SetCrashReportCustomString(customString);
        }
        public void SetCrashReportCustomStack(string customStack)
        {
            _logger.LogInformation("Crash Report StackTrace: {customStack}}", customStack);
            _debugManager.SetCrashReportCustomStack(customStack);
        }

        public void WriteDebugLineOnScreen(string message)
        {
            _logger.LogDebug("Message: {message}}", message);
            _debugManager.WriteDebugLineOnScreen(message);
        }

        public void Assert(bool condition, string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!condition)
                _logger.LogError("Assert Failed!: {message}; CallerFilePath: {callerFile}; CallerMemberName: {callerMethod}; CallerLineNumber: {callerLine}", message, callerFile, callerMethod, callerLine);
            _debugManager.Assert(condition, message, callerFile, callerMethod, callerLine);
        }

        public void RenderDebugLine(Vec3 position, Vec3 direction, uint color = 4294967295, bool depthCheck = false, float time = 0) =>
            _debugManager.RenderDebugLine(position, direction, color, depthCheck, time);
        public void RenderDebugSphere(Vec3 position, float radius, uint color = 4294967295, bool depthCheck = false, float time = 0) =>
            _debugManager.RenderDebugSphere(position, radius, color, depthCheck, time);
        public void RenderDebugFrame(MatrixFrame frame, float lineLength, float time = 0) => _debugManager.RenderDebugFrame(frame, lineLength, time);
        public void RenderDebugText(float screenX, float screenY, string text, uint color = 4294967295, float time = 0) =>
            _debugManager.RenderDebugText(screenX, screenY, text, color, time);

        public void WatchVariable(string name, object value) => _debugManager.WatchVariable(name, value);

        public void BeginTelemetryScope(TelemetryLevelMask levelMask, string scopeName) => _debugManager.BeginTelemetryScope(levelMask, scopeName);
        public void EndTelemetryScope() => _debugManager.EndTelemetryScope();

        public Vec3 GetDebugVector() => _debugManager.GetDebugVector();
    }
}
