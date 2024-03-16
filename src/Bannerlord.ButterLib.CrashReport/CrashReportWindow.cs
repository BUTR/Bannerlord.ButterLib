using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.OpenGL;
using Bannerlord.ButterLib.CrashReportWindow.Renderer;
using Bannerlord.ButterLib.CrashReportWindow.UnsafeUtils;
using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashReportWindow;

public class CrashReportWindow : IDisposable
{
    public static void ShowAndWait(
        Exception exception,
        IList<LogSource> logSources,
        Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload,
        MethodInfo? bewFinalizer)
    {
        var metadata = new Dictionary<string, string>
        {
            {"METADATA:TW_ConfigName", TaleWorlds.Library.Common.ConfigName},
        };
        if (Process.GetCurrentProcess().ParentProcess() is { } pProcess)
        {
            metadata.Add("Parent_Process_Name", pProcess.ProcessName);
            metadata.Add("Parent_Process_File_Version", pProcess.MainModule?.FileVersionInfo.FileVersion ?? "0");
        }

        var filter = new StacktraceFilter(bewFinalizer);
        var helper = new CrashReportInfoHelper();
        var harmonyProvider = new HarmonyProvider();
        var crashReport = CrashReportInfo.Create(exception, metadata, filter, helper, helper, helper, harmonyProvider);

        var crashReportModel = CrashReportInfo.ToModel(crashReport, helper, helper, helper, helper, helper, helper);

        try
        {
            using var window = new CrashReportWindow();
            window.Init(crashReportModel, logSources, upload);
        }
        //catch (Exception) { /* ignore */ }
        catch (Exception e)
        {
            ;
        }
    }

    private ImGuiController? imGuiController;

    private void Init(CrashReportModel crashReportModel, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload)
    {
        if (GLFW.glfwInit() == 0) throw new Exception("glfwInit");
        GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MAJOR, 4);
        GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MINOR, 6);
        GLFW.glfwWindowHint(GLFW.GLFW_OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE);

        //var monitor = GLFW.glfwGetPrimaryMonitor();
        //var mode = GLFW.glfwGetVideoMode(monitor);
        //window = GLFW.glfwCreateWindow(mode.width, mode.height, "BannerlordCrash Report", IntPtr.Zero, IntPtr.Zero);
        var window_ = new GLFW.GLFWWindowPtr(in GLFW.glfwCreateWindow(800, 600, "Bannerlord Crash Report\0"u8, IntPtr.Zero, IntPtr.Zero));
        if (window_.Ptr == IntPtr.Zero)
        {
            //GLFW.glfwTerminate();
            throw new Exception("glfwCreateWindow");
        }

        GLFW.glfwMakeContextCurrent(in window_.Ref);

        ref var tt = ref GLFW.glfwGetRequiredInstanceExtensions(out var count);
        var ttSpan = Utf8ZPtr.AsSpan(ref tt, (int) count);
        var ttStrings = ttSpan.ToArray().Select(s => s.AsUtf16String()).ToArray();

        imGuiController = new ImGuiController(window_);
        imGuiController.Init();

        GLFW.glfwFocusWindow(in window_.Ref);

        var shouldClose = false;
        void CloseAction() => shouldClose = true;
        var imGuiRenderer = new ImGuiRenderer(crashReportModel, logSources, upload, CloseAction);

        var targetElapsedTime = TimeSpan.FromMilliseconds(1000D / 60.0D);
        var maxElapsedTime = TimeSpan.FromMilliseconds(500);
        var accumulatedElapsedTime = TimeSpan.Zero;
        var previousTicks = 0L;
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var loopTimer = Stopwatch.StartNew();

        void GlfwEvents(ref readonly GLFW.GLFWWindow window)
        {
            GLFW.glfwPollEvents();
            if (GLFW.glfwGetKey(in window, GLFW.GLFW_KEY_ESCAPE) == GLFW.GLFW_PRESS)
                GLFW.glfwSetWindowShouldClose(in window, GLFW.GLFW_TRUE);
        }

        void DoUpdate(ref readonly GLFW.GLFWWindow window)
        {
            GlfwEvents(in window);
            imGuiController?.Update();
        }

        void DoDraw(ref readonly GLFW.GLFWWindow window)
        {
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);
            imGuiController?.Render();
            GLFW.glfwSwapBuffers(in window);
        }

        // Kinda taken from MonoGame
        void WaitFixedTime()
        {
        RetryLoop:
            var currentTicks = loopTimer.Elapsed.Ticks;
            accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - previousTicks);
            previousTicks = currentTicks;

            if (accumulatedElapsedTime < targetElapsedTime)
            {
                var sleepTime = (targetElapsedTime - accumulatedElapsedTime).TotalMilliseconds;

                if (isWindows)
                    TimerHelper.SleepForNoMoreThan(sleepTime);

                if (!isWindows && sleepTime >= 2.0)
                    Thread.Sleep(1);

                // Keep looping until it's time to perform the next update
                goto RetryLoop;
            }

            // Do not allow any update to take longer than our maximum.
            if (accumulatedElapsedTime > maxElapsedTime)
                accumulatedElapsedTime = maxElapsedTime;
        }

        while (!shouldClose && GLFW.glfwWindowShouldClose(in window_.Ref) == GLFW.GLFW_FALSE)
        {
            DoUpdate(in window_.Ref);

            imGuiRenderer.Render();

            DoDraw(in window_.Ref);

            //WaitFixedTime();

            loopTimer.Restart();
        }

        GLFW.glfwDestroyWindow(in window_.Ref);
    }

    public void Dispose()
    {
        imGuiController?.Dispose();
    }
}