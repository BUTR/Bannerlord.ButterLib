using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.OpenGL;
using Bannerlord.ButterLib.CrashReportWindow.Renderer;
using Bannerlord.ButterLib.CrashReportWindow.Utils;
using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

namespace Bannerlord.ButterLib.CrashReportWindow;

public class CrashReportWindow : IDisposable
{
    public static void ShowAndWait(
        Exception exception,
        ICollection<LogSource> logSources,
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
        
        var crashReportModel = CrashReportInfo.ToModel(crashReport, helper, helper, helper, helper, helper, helper, helper);

        try
        {
            using var window = new CrashReportWindow();
            window.Init(crashReportModel, logSources, upload);
        }
        catch (Exception) { /* ignore */ }
    }

    private IntPtr window;
    private ImGuiController? imGuiController;

    private void Init(CrashReportModel crashReportModel, ICollection<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload)
    {
        if (GLFW.glfwInit() == 0) throw new Exception("glfwInit");
        GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MAJOR, 4);
        GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MINOR, 6);
        GLFW.glfwWindowHint(GLFW.GLFW_OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE);

        //var monitor = GLFW.glfwGetPrimaryMonitor();
        //var mode = GLFW.glfwGetVideoMode(monitor);
        //window = GLFW.glfwCreateWindow(mode.width, mode.height, "BannerlordCrash Report", IntPtr.Zero, IntPtr.Zero);
        window = GLFW.glfwCreateWindow(800, 600, "Bannerlord Crash Report", IntPtr.Zero, IntPtr.Zero);
        if (window == IntPtr.Zero)
        {
            //GLFW.glfwTerminate();
            throw new Exception("glfwCreateWindow");
        }

        GLFW.glfwMakeContextCurrent(window);
        GL.LoadEntryPoints();
        imGuiController = new ImGuiController(window);
        imGuiController.Init();

        GLFW.glfwFocusWindow(window);
        void CloseAction() => GLFW.glfwSetWindowShouldClose(window, GLFW.GLFW_TRUE);
        var imGuiRenderer = new ImGuiRenderer(crashReportModel, logSources, upload, CloseAction);
        
        var stopwatch = Stopwatch.StartNew();
        while (GLFW.glfwWindowShouldClose(window) == 0)
        {
            const int maxFrameTime = 1000 / 60;
            var elapsed = (int) stopwatch.Elapsed.TotalMilliseconds;
            var toSkip = maxFrameTime - elapsed;
            if (toSkip > 0) Thread.Sleep(toSkip);
            
            GlfwEvents();
            imGuiController.Update();
            
            imGuiRenderer.Render();
            
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);
            imGuiController.Render();
            GLFW.glfwSwapBuffers(window);
            stopwatch.Restart();
        }
        GLFW.glfwDestroyWindow(window);
    }

    private void GlfwEvents()
    {
        GLFW.glfwPollEvents();
        if (GLFW.glfwGetKey(window, GLFW.GLFW_KEY_ESCAPE) == GLFW.GLFW_PRESS)
            GLFW.glfwSetWindowShouldClose(window, GLFW.GLFW_TRUE);
    }

    public void Dispose()
    {
        imGuiController?.Dispose();
    }
}