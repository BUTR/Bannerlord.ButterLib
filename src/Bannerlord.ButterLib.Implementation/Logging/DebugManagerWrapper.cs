using Microsoft.Extensions.Logging;

using System;
using System.Runtime.CompilerServices;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Implementation.Logging
{
    /// <summary>
    /// Wraps a <see cref="IDebugManager"/>. Doubles the logs into out logging system
    /// but still keeps the original execution flow.
    /// </summary>
    internal sealed class DebugManagerWrapper : IDebugManager
    {
        public IDebugManager OriginalDebugManager { get; }

        private readonly ILogger _logger;
        private readonly ILogger _debugManagerLogger;

        public DebugManagerWrapper(IDebugManager debugManager, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(DebugManagerWrapper));
            OriginalDebugManager = debugManager;
            _debugManagerLogger =  loggerFactory.CreateLogger(debugManager.GetType());
        }

        public void ShowWarning(string message)
        {
            _debugManagerLogger.LogWarning(message);
            OriginalDebugManager.ShowWarning(message);
        }

        public void DisplayDebugMessage(string message)
        {
            _debugManagerLogger.LogDebug("{message}", message);
            OriginalDebugManager.DisplayDebugMessage(message);
        }

        public void Print(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416)
        {
            // logLevel is not used by the game right now.
            _debugManagerLogger.LogInformation("{message}", message);
            OriginalDebugManager.Print(message, logLevel, color, debugFilter);
        }
        public void PrintError(string error, string stackTrace, ulong debugFilter = 17592186044416)
        {
            _debugManagerLogger.LogError("{error}{nl}{stackTrace}", error, Environment.NewLine, stackTrace);
            OriginalDebugManager.PrintError(error, stackTrace, debugFilter);
        }
        public void PrintWarning(string warning, ulong debugFilter = 17592186044416)
        {
            _debugManagerLogger.LogWarning("{warning}", warning);
            OriginalDebugManager.PrintWarning(warning, debugFilter);
        }

        public void SetCrashReportCustomString(string customString)
        {
            _debugManagerLogger.LogCritical("Crash Report: {customString}", customString);
            OriginalDebugManager.SetCrashReportCustomString(customString);
        }
        public void SetCrashReportCustomStack(string customStack)
        {
            _debugManagerLogger.LogCritical("Crash Report StackTrace: {customStack}", customStack);
            OriginalDebugManager.SetCrashReportCustomStack(customStack);
        }

        public void WriteDebugLineOnScreen(string message)
        {
            _debugManagerLogger.LogDebug("{message}", message);
            OriginalDebugManager.WriteDebugLineOnScreen(message);
        }

        public void Assert(bool condition, string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!condition)
                _debugManagerLogger.LogDebug("Assert Failed!: {message}; CallerFilePath: {callerFile}; CallerMemberName: {callerMethod}; CallerLineNumber: {callerLine}", message, callerFile, callerMethod, callerLine);
            // ReSharper disable ExplicitCallerInfoArgument
            OriginalDebugManager.Assert(condition, message, callerFile, callerMethod, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public void SilentAssert(bool condition, string message = "", bool getDump = false, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!condition)
                _debugManagerLogger.LogDebug("Silent Assert Failed!: {message}; CallerFilePath: {callerFile}; CallerMemberName: {callerMethod}; CallerLineNumber: {callerLine}", message, callerFile, callerMethod, callerLine);
            // ReSharper disable ExplicitCallerInfoArgument
            OriginalDebugManager.SilentAssert(condition, message, getDump, callerFile, callerMethod, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public void RenderDebugLine(Vec3 position, Vec3 direction, uint color = 4294967295, bool depthCheck = false, float time = 0) =>
            OriginalDebugManager.RenderDebugLine(position, direction, color, depthCheck, time);
        public void RenderDebugSphere(Vec3 position, float radius, uint color = 4294967295, bool depthCheck = false, float time = 0) =>
            OriginalDebugManager.RenderDebugSphere(position, radius, color, depthCheck, time);
        public void RenderDebugFrame(MatrixFrame frame, float lineLength, float time = 0) => OriginalDebugManager.RenderDebugFrame(frame, lineLength, time);
        public void RenderDebugText(float screenX, float screenY, string text, uint color = 4294967295, float time = 0) =>
            OriginalDebugManager.RenderDebugText(screenX, screenY, text, color, time);

        public void WatchVariable(string name, object value) => OriginalDebugManager.WatchVariable(name, value);

        public void BeginTelemetryScope(TelemetryLevelMask levelMask, string scopeName) => OriginalDebugManager.BeginTelemetryScope(levelMask, scopeName);
        public void EndTelemetryScope() => OriginalDebugManager.EndTelemetryScope();

        public Vec3 GetDebugVector() => OriginalDebugManager.GetDebugVector();

#if e152 || e153 || e154 || e155 || e156 || e157 || e158 || e159 || e1510 || e160 || e161 || e162 || e163 || e164 || e165 || e170 || e171
        public void ShowMessageBox(string lpText, string lpCaption, uint uType)
        {
            OriginalDebugManager.ShowMessageBox(lpText, lpCaption, uType);
        }
#endif
#if e164 || e165 || e170 || e171
        public void ShowError(string message)
        {
            _debugManagerLogger.LogError("{message}", message);
            OriginalDebugManager.ShowError(message);
        }
#endif
#if e165 || e170 || e171
        public void BeginTelemetryScopeBaseLevel(TelemetryLevelMask levelMask, string scopeName) =>
            OriginalDebugManager.BeginTelemetryScopeBaseLevel(levelMask, scopeName);
        public void EndTelemetryScopeBaseLevel() => OriginalDebugManager.EndTelemetryScopeBaseLevel();
#endif
#if e170 || e171
        public void RenderDebugText3D(Vec3 position, string text, uint color = uint.MaxValue, int screenPosOffsetX = 0, int screenPosOffsetY = 0, float time = 0) =>
            OriginalDebugManager.RenderDebugText3D(position, text, color, screenPosOffsetX, screenPosOffsetY, time);
#endif
    }
}