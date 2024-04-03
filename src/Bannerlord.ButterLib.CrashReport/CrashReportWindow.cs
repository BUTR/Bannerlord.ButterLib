using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.CrashReportWindow.Controller;
using Bannerlord.ButterLib.CrashReportWindow.Extensions;
using Bannerlord.ButterLib.CrashReportWindow.ImGui;
using Bannerlord.ButterLib.CrashReportWindow.Renderer;
using Bannerlord.ButterLib.CrashReportWindow.Utils;

using BUTR.CrashReport;
using BUTR.CrashReport.Bannerlord;
using BUTR.CrashReport.Models;

using Silk.NET.Core.Loader;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using TaleWorlds.Library;

namespace Bannerlord.ButterLib.CrashReportWindow;

public readonly ref struct CrashReportWindow
{
    static CrashReportWindow()
    {
        if (PathResolver.Default is DefaultPathResolver pr)
        {
            var modulePath = ModuleInfoHelper.GetModulePath(typeof(CrashReportWindow))!;
            pr.Resolvers =
            [
                path => new[]
                {
                    Path.Combine(modulePath, "bin", Common.ConfigName, path)
                }
            ];
        }

        GlfwInput.RegisterPlatform();
        GlfwWindowing.RegisterPlatform();
    }

    public static void ShowAndWait(Exception exception, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload, MethodInfo? bewFinalizer)
    {
        var metadata = new Dictionary<string, string>
        {
            { "METADATA:TW_ConfigName", Common.ConfigName },
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
            ShowAndWait(crashReportModel, logSources, upload);
        }
        //catch (Exception) { /* ignore */ }
        catch (Exception e)
        {
            ;
        }
    }

    private static void ShowAndWait(CrashReportModel crashReportModel, IList<LogSource> logSources, Func<CrashReportModel, IEnumerable<LogSource>, Task<(bool, string)>> upload)
    {
        var window = Window.Create(WindowOptions.Default with { VSync = false });

        var gl = default(GL)!;
        var controller = default(ImGuiController)!;
        var imGuiRenderer = default(ImGuiRenderer)!;

        window.Load += () =>
        {
            var imgui = new CmGui();
            var inputContext = window.CreateInput();

            gl = window.CreateOpenGL();
            controller = new ImGuiController(imgui, gl, window, inputContext);
            imGuiRenderer = new ImGuiRenderer(imgui, crashReportModel, logSources, upload, window.Close);

            controller.Init();
        };
        window.FramebufferResize += s =>
        {
            gl.Viewport(s);
        };
        window.Render += delta =>
        {
            if (window.IsClosing) return;

            controller.Update(delta);

            gl.Clear(ClearBufferMask.ColorBufferBit);

            imGuiRenderer.Render();

            controller.Render();
        };

        DoLoop(window);

        controller.Dispose();
        window.Dispose();
    }

    private static void DoLoop(IWindow window)
    {
        window.Initialize();
        window.Run(() =>
        {
            window.DoEvents();

            if (!window.IsClosing)
                window.DoUpdate();

            if (!window.IsClosing)
                window.DoRender();
        });
        window.DoEvents();
    }
}