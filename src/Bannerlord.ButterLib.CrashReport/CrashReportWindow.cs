using Bannerlord.ButterLib.CrashReportWindow.Controller;
using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.ImGui;
using Bannerlord.ButterLib.CrashReportWindow.OpenGL;
using Bannerlord.ButterLib.CrashReportWindow.Renderer;
using Bannerlord.ButterLib.CrashReportWindow.Utils;
using Bannerlord.ButterLib.CrashReportWindow.Windowing;

using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Bannerlord.ButterLib.CrashReportWindow;

using static Glfw;

public readonly ref struct CrashReportWindow
{
    public static void ShowAndWait(Exception exception, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload, MethodInfo? bewFinalizer)
    {
        var metadata = new Dictionary<string, string>
        {
            { "METADATA:TW_ConfigName", TaleWorlds.Library.Common.ConfigName },
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
            var window = new CrashReportWindow();
            window.ShowAndWait(crashReportModel, logSources, upload);
        }
        //catch (Exception) { /* ignore */ }
        catch (Exception e)
        {
            ;
        }
    }

    private void ShowAndWait(CrashReportModel crashReportModel, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload)
    {
        var imgui = new CmGui(CmGui.LoadFunction);
        var glfw = new Glfw(Glfw.LoadFunction);

        if (glfw.Init())
            glfw.CheckErrorIgnoreInit(); // for some reason recreating the window throws not initialized
        else
            glfw.CheckError();

        glfw.WindowHint(GLFW_OPENGL_FORWARD_COMPAT, GLFW_TRUE);
        glfw.WindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
        glfw.WindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
        glfw.WindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
        glfw.WindowHint(GLFW_RESIZABLE, GLFW_TRUE);
        glfw.CheckError();

        //var monitor = GLFW. glfw.GetPrimaryMonitor();
        //var mode = GLFW. glfw.GetVideoMode(monitor);
        //window = GLFW. glfw.CreateWindow(mode.width, mode.height, "BannerlordCrash Report", IntPtr.Zero, IntPtr.Zero);
        var window = glfw.CreateWindow(800, 600, "Bannerlord Crash Report\0"u8, IntPtr.Zero, in GlfwWindowHandle.NullRef());
        glfw.CheckError();

        glfw.MakeContextCurrent(in window.Handle);

        var gl = new Gl(glfw.GetProcAddress);

        glfw.SwapInterval(0); // Turns VSync off.

        //ref var tt = ref glfw.GetRequiredInstanceExtensions(out var count);
        //var ttSpan = Utf8ZPtr.AsSpan(ref tt, (int) count);
        //var ttStrings = ttSpan.ToArray().Select(s => s.AsUtf16String()).ToArray();

        using var imGuiController = new ImGuiController(imgui, glfw, gl, window);
        imGuiController.Init();

        glfw.FocusWindow(in window.Handle);

        var shouldClose = false;
        void CloseAction() => shouldClose = true;
        var imGuiRenderer = new ImGuiRenderer(imgui, crashReportModel, logSources, upload, CloseAction);

        var targetElapsedTime = TimeSpan.FromMilliseconds(1000D / 60.0D);
        var maxElapsedTime = TimeSpan.FromMilliseconds(500);
        var accumulatedElapsedTime = TimeSpan.Zero;
        var previousTicks = 0L;
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var loopTimer = Stopwatch.StartNew();

        void GlfwEvents()
        {
            glfw.PollEvents();
            if (glfw.GetKey(in window.Handle, GLFW_KEY_ESCAPE) == GLFW_PRESS)
                glfw.SetWindowShouldClose(in window.Handle, GLFW_TRUE);
        }

        void DoUpdate(float delta)
        {
            GlfwEvents();
            imGuiController?.Update(delta);
        }

        void DoDraw()
        {
            gl.Clear(Gl.GL_COLOR_BUFFER_BIT);
            imGuiController?.Render();
            glfw.SwapBuffers(in window.Handle);
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

        double time = 0;
        while (!shouldClose && glfw.WindowShouldClose(in window.Handle) == GLFW_FALSE)
        {
            var currentTime = glfw.GetTime();
            var delta = time > 0.0 ? (float) (currentTime - time) : 1.0f / 60.0f;
            time = currentTime;

            DoUpdate(delta);

            imGuiRenderer.Render();

            DoDraw();

            //WaitFixedTime();

            loopTimer.Restart();
        }

        glfw.DestroyWindow(in window.Handle);
        glfw.CheckError();
        glfw.Terminate();
        glfw.CheckError();
    }
}