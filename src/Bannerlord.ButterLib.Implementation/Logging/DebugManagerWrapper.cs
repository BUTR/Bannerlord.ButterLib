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
            _debugManagerLogger = loggerFactory.CreateLogger(debugManager.GetType());
        }

        public void ShowWarning(string message)
        {
            _debugManagerLogger.LogWarning("{Message}", message);
            OriginalDebugManager.ShowWarning(message);
        }

        public void DisplayDebugMessage(string message)
        {
            _debugManagerLogger.LogDebug("{Message}", message);
            OriginalDebugManager.DisplayDebugMessage(message);
        }

        public void Print(string message, int logLevel = 0, Debug.DebugColor color = Debug.DebugColor.White, ulong debugFilter = 17592186044416)
        {
            // logLevel is not used by the game right now.
            _debugManagerLogger.LogInformation("{Message}", message);
            OriginalDebugManager.Print(message, logLevel, color, debugFilter);
        }
        public void PrintError(string error, string stackTrace, ulong debugFilter = 17592186044416)
        {
            _debugManagerLogger.LogError("{Error}{NL}{StackTrace}", error, Environment.NewLine, stackTrace);
            OriginalDebugManager.PrintError(error, stackTrace, debugFilter);
        }
        public void PrintWarning(string warning, ulong debugFilter = 17592186044416)
        {
            _debugManagerLogger.LogWarning("{Warning}", warning);
            OriginalDebugManager.PrintWarning(warning, debugFilter);
        }

        public void SetCrashReportCustomString(string customString)
        {
            _debugManagerLogger.LogCritical("Crash Report: {CustomString}", customString);
            OriginalDebugManager.SetCrashReportCustomString(customString);
        }
        public void SetCrashReportCustomStack(string customStack)
        {
            _debugManagerLogger.LogCritical("Crash Report StackTrace: {CustomStack}", customStack);
            OriginalDebugManager.SetCrashReportCustomStack(customStack);
        }

        public void SetTestModeEnabled(bool testModeEnabled) => OriginalDebugManager.SetTestModeEnabled(testModeEnabled);

        public void AbortGame() => OriginalDebugManager.AbortGame();

        public void DoDelayedexit(int returnCode) => OriginalDebugManager.DoDelayedexit(returnCode);

        public void EndTelemetryScopeBaseLevelInternal() => OriginalDebugManager.EndTelemetryScopeBaseLevelInternal();

        public void WriteDebugLineOnScreen(string message)
        {
            _debugManagerLogger.LogDebug("{Message}", message);
            OriginalDebugManager.WriteDebugLineOnScreen(message);
        }

        public void Assert(bool condition, string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!condition)
                _debugManagerLogger.LogDebug("Assert Failed!: {Message}; CallerFilePath: {CallerFile}; CallerMemberName: {CallerMethod}; CallerLineNumber: {CallerLine}", message, callerFile, callerMethod, callerLine);
            // ReSharper disable ExplicitCallerInfoArgument
            OriginalDebugManager.Assert(condition, message, callerFile, callerMethod, callerLine);
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public void SilentAssert(bool condition, string message = "", bool getDump = false, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            if (!condition)
                _debugManagerLogger.LogDebug("Silent Assert Failed!: {Message}; CallerFilePath: {CallerFile}; CallerMemberName: {CallerMethod}; CallerLineNumber: {CallerLine}", message, callerFile, callerMethod, callerLine);
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

#if e180
        public void RenderDebugRectWithColor(float left, float bottom, float right, float top, uint color = 4294967295)  =>
            OriginalDebugManager.RenderDebugRectWithColor(left, bottom, right, top, color);
#endif

        public void WatchVariable(string name, object value) => OriginalDebugManager.WatchVariable(name, value);

        public void BeginTelemetryScopeInternal(TelemetryLevelMask levelMask, string scopeName) => OriginalDebugManager.BeginTelemetryScopeInternal(levelMask, scopeName);
        public void BeginTelemetryScopeBaseLevelInternal(TelemetryLevelMask levelMask, string scopeName) => OriginalDebugManager.BeginTelemetryScopeBaseLevelInternal(levelMask, scopeName);
        public void EndTelemetryScopeInternal() => OriginalDebugManager.EndTelemetryScopeInternal();

        public Vec3 GetDebugVector() => OriginalDebugManager.GetDebugVector();

        public void ShowMessageBox(string lpText, string lpCaption, uint uType)
        {
            OriginalDebugManager.ShowMessageBox(lpText, lpCaption, uType);
        }

        public void ShowError(string message)
        {
            _debugManagerLogger.LogError("{Message}", message);
            OriginalDebugManager.ShowError(message);
        }

        public void RenderDebugText3D(Vec3 position, string text, uint color = uint.MaxValue, int screenPosOffsetX = 0, int screenPosOffsetY = 0, float time = 0) =>
            OriginalDebugManager.RenderDebugText3D(position, text, color, screenPosOffsetX, screenPosOffsetY, time);

    }
}