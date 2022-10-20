using Bannerlord.BUTR.Shared.Extensions;
using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.ExceptionHandler.WinForms;
using Bannerlord.ButterLib.Logger;
using Bannerlord.ModuleManager;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using TWCommon = TaleWorlds.Library.Common;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal static class HtmlBuilder
    {
        private static readonly string NL = Environment.NewLine;

        public static void BuildAndShow(CrashReport crashReport)
        {
            using var form = new HtmlCrashReportForm(crashReport);
            form.ShowDialog();
        }

        public static string Build(CrashReport crashReport, string miniDump) => @$"
<html>
  <head>
    <title>Bannerlord Crash Report</title>
    <meta charset='utf-8'>
    <game version='{ApplicationVersionHelper.GameVersionStr()}'>
    <report id='{crashReport.Id}' version='5'>
    <style>
        .headers {{
            font-family: ""Consolas"", monospace;
        }}
        .root-container {{
            font-family: ""Consolas"", monospace;
            font-size: small;

            margin: 5px;
            background-color: white;
            border: 1px solid grey;
            padding: 5px;
        }}
        .headers-container {{
            display: none;
        }}
        .modules-container {{
            margin: 5px;
            background-color: #ffffe0;
            border: 1px solid grey;
            padding: 5px;
        }}
        .submodules-container {{
            margin: 5px;
            border: 1px solid grey;
            background-color: #f8f8e7;
            padding: 5px;
        }}
        .modules-official-container {{
            margin: 5px;
            background-color: #f4fcdc;
            border: 1px solid grey;
            padding: 5px;
        }}
        .submodules-official-container {{
            margin: 5px;
            border: 1px solid grey;
            background-color: #f0f4e4;
            padding: 5px;
        }}
        .modules-invalid-container {{
            margin: 5px;
            background-color: #ffefd5;
            border: 1px solid grey;
            padding: 5px;
        }}
        .submodules-invalid-container {{
            margin: 5px;
            border: 1px solid grey;
            background-color: #f5ecdf;
            padding: 5px;
        }}
    </style>
  </head>
  <body style='background-color: #ececec;'>
    <table style='width: 100%;'>
      <tbody>
        <tr>
          <td style='width: 80%;'>
            <div>
              <b>Bannerlord has encountered a problem and will close itself.</b>
              <br/>
              This is a community Crash Report. Please save it and use it for reporting the error. Do not provide screenshots, provide the report!
              <br/>
              Most likely this error was caused by a custom installed module.
              <br/>
              <br/>
              If you were in the middle of something, the progress might be lost.
              <br/>
            </div>
          </td>
           <td>
              <div style='float:right; margin-left:10px;'>
              <label>Without Color:</label>
              <input type='checkbox' onclick='changeBackgroundColor(this)'>
              <br/>
              <br/>
              <label>Font Size:</label>
              <select class='input' onchange='changeFontSize(this);'>
                <option value='1.0em' selected='selected'>Standard</option>
                <option value='0.9em'>Medium</option>
                <option value='0.8em'>Small</option>
              </select>
{(string.IsNullOrEmpty(miniDump) ? "" : @"
<![if !IE]>
              <br/>
              <br/>
              <button onclick='minidump(this)'>Get MiniDump</button>
<![endif]>
")}
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""exception"")'>+ Exception</a></h2>
      <div id='exception' class='headers-container'>
      {GetRecursiveExceptionHtml(crashReport.Exception)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""enhanced-stacktrace"")'>+ Enhanced Stacktrace</a></h2>
      <div id='enhanced-stacktrace' class='headers-container'>
      {GetEnhancedStacktraceHtml(crashReport)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""involved-modules"")'>+ Involved Modules</a></h2>
      <div id='involved-modules' class='headers-container'>
      {GetInvolvedModuleListHtml(crashReport)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""installed-modules"")'>+ Installed Modules</a></h2>
      <div id='installed-modules' class='headers-container'>
      {GetModuleListHtml(crashReport)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""assemblies"")'>+ Assemblies</a></h2>
      <div id='assemblies' class='headers-container'>
      <label>Hide: </label>
      <label><input type='checkbox' onclick='showHideByClassName(this, ""tw_assembly"")'> Game Core</label>
      <label><input type='checkbox' onclick='showHideByClassName(this, ""sys_assembly"")'> System</label>
      <label><input type='checkbox' onclick='showHideByClassName(this, ""module_assembly"")'> Modules</label>
      <label><input type='checkbox' onclick='showHideByClassName(this, ""unclas_assembly"")'> Unclassified</label>
      {GetAssemblyListHtml(crashReport)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""harmony-patches"")'>+ Harmony Patches</a></h2>
      <div id='harmony-patches' class='headers-container'>
      {GetHarmonyPatchesListHtml(crashReport)}
      </div>
    </div>
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""log-files"")'>+ Log Files</a></h2>
      <div id='log-files' class='headers-container'>
      {GetLogFilesListHtml(crashReport)}
      </div>
    </div>
    <div class='root-container' style='display:none;'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""mini-dump"")'>+ Mini Dump</a></h2>
      <div id='mini-dump' class='headers-container'>
      {miniDump}
      </div>
    </div>
{(string.IsNullOrEmpty(miniDump) ? "" : @"
<![if !IE]>
    <script src=""https://cdn.jsdelivr.net/pako/1.0.3/pako_inflate.min.js""></script>
<![endif]>
")}
    <script>
      function showHideById(element, id) {{
          if (document.getElementById(id).style.display === 'block') {{
              document.getElementById(id).style.display = 'none';
              element.innerHTML = element.innerHTML.replace('-', '+');
          }} else {{
              document.getElementById(id).style.display = 'block';
              element.innerHTML = element.innerHTML.replace('+', '-');
          }}
      }}
      function showHideByClassName(element, className) {{
          var list = document.getElementsByClassName(className)
          for (var i = 0; i < list.length; i++) {{
              list[i].style.display = (element.checked) ? 'none' : 'list-item';
          }}
      }}
      function setBackgroundColorByClassName(className, color) {{
          var list = document.getElementsByClassName(className);
          for (var i = 0; i < list.length; i++) {{
            list[i].style.backgroundColor = color;
          }}
      }}
      function changeFontSize(fontSize) {{
          document.getElementById('exception').style.fontSize = fontSize.value;
          document.getElementById('involved-modules').style.fontSize = fontSize.value;
          document.getElementById('installed-modules').style.fontSize = fontSize.value;
          document.getElementById('assemblies').style.fontSize = fontSize.value;
          document.getElementById('harmony-patches').style.fontSize = fontSize.value;
      }}
      function changeBackgroundColor(element) {{
          document.body.style.backgroundColor = (!element.checked) ? '#ececec' : 'white';
          setBackgroundColorByClassName('headers-container', (!element.checked) ? 'white' : 'white');
          setBackgroundColorByClassName('modules-container', (!element.checked) ? '#ffffe0' : 'white');
          setBackgroundColorByClassName('submodules-container', (!element.checked) ? '#f8f8e7' : 'white');
          setBackgroundColorByClassName('modules-official-container', (!element.checked) ? '#f4fcdc' : 'white');
          setBackgroundColorByClassName('submodules-official-container', (!element.checked) ? '#f0f4e4' : 'white');
          setBackgroundColorByClassName('modules-invalid-container', (!element.checked) ? '#ffefd5' : 'white');
          setBackgroundColorByClassName('submodules-invalid-container', (!element.checked) ? '#f5ecdf' : 'white');
      }}
      function minidump(element) {{
          var base64 = document.getElementById('mini-dump').innerText.trim();
          //var binData = Uint8Array.from(atob(base64), c => c.charCodeAt(0));
          var binData = new Uint8Array(atob(base64).split('').map(function(x){{return x.charCodeAt(0);}}));
          var result = window.pako.inflate(binData);

          var a = document.createElement('a');
          var blob = new Blob([result]);
          a.href = window.URL.createObjectURL(blob);
          a.download = ""crashdump.dmp"";
          a.click();
        }}
    </script>
  </body>
</html>";

        private static string GetRecursiveExceptionHtml(Exception ex) => new StringBuilder()
            .AppendLine("Exception information")
            .AppendLine($"<br/>{NL}Type: {ex.GetType().FullName}")
            .AppendLine(!string.IsNullOrWhiteSpace(ex.Message) ? $"</br>{NL}Message: {ex.Message}" : string.Empty)
            .AppendLine(!string.IsNullOrWhiteSpace(ex.Source) ? $"</br>{NL}Source: {ex.Source}" : string.Empty)
            .AppendLine(!string.IsNullOrWhiteSpace(ex.StackTrace) ? $"</br>{NL}CallStack:{NL}</br>{NL}<ol>{NL}<li>{string.Join($"</li>{NL}<li>", ex.StackTrace.Split(new[] { NL }, StringSplitOptions.RemoveEmptyEntries))}</li>{NL}</ol>" : string.Empty)
            .AppendLine(ex.InnerException != null ? $"<br/>{NL}<br/>{NL}Inner {GetRecursiveExceptionHtml(ex.InnerException)}" : string.Empty)
            .ToString();

        private static string GetEnhancedStacktraceHtml(CrashReport crashReport)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<ul>");
            foreach (var stacktrace in crashReport.Stacktrace.GroupBy(x => x.StackFrameDescription))
            {
                sb.Append("<li>")
                    .Append($"Frame: {stacktrace.Key}</br>")
                    .Append("<ul>");
                foreach (var (method, module, _) in stacktrace)
                {
                    sb.Append("<li>")
                        .Append($"Module: {(module is null ? "UNKNOWN" : module.Id)}</br>")
                        .Append($"Method: {method.FullDescription()}</br>")
                        .Append("</li>");
                }
                sb.AppendLine("</ul></li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static string GetInvolvedModuleListHtml(CrashReport crashReport)
        {
            static bool FilterButterLib(StacktraceEntry stacktraceEntry)
            {
                if (stacktraceEntry.ModuleInfo?.Id == "Bannerlord.ButterLib")
                {
                    if (stacktraceEntry.Method == BEWPatch.FinalizerMethod)
                    {
                        return false;
                    }
                }

                return true;
            }

            // Do not show Bannerlord.Harmony if it's the only one involved module.
            if (crashReport.Stacktrace.Count == 1 && crashReport.Stacktrace[0].ModuleInfo?.Id == "Bannerlord.Harmony")
                return "<ul></ul>";

            var sb = new StringBuilder();
            sb.AppendLine("<ul>");
            foreach (var stacktrace in crashReport.Stacktrace.Where(FilterButterLib).GroupBy(m => m.ModuleInfo))
            {
                var module = stacktrace.Key;
                if (module is null) continue;

                sb.Append("<li>")
                    .Append($"<a href='javascript:;' onclick='document.getElementById(\"{module.Id}\").scrollIntoView(false)'>").Append(module.Id).Append("</a></br>")
                    .Append("<ul>");
                foreach (var (method, _, stackFrameDescription) in stacktrace)
                {
                    // Ignore blank transpilers used to force the jitter to skip inlining
                    if (method.Name == "BlankTranspiler") continue;
                    sb.Append("<li>")
                        .Append($"Method: {method.FullDescription()}</br>")
                        .Append($"Frame: {stackFrameDescription}</br>")
                        .Append("</li>");
                }
                sb.AppendLine("</ul></li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static string GetModuleListHtml(CrashReport crashReport)
        {
            var moduleBuilder = new StringBuilder();
            var subModulesBuilder = new StringBuilder();
            var assembliesBuilder = new StringBuilder();
            var tagsBuilder = new StringBuilder();
            var additionalAssembliesBuilder = new StringBuilder();
            var dependenciesBuilder = new StringBuilder();


            void AppendDependencies(ModuleInfoExtended module)
            {
                var deps = new Dictionary<string, string>();
                var tmp = new StringBuilder();
                foreach (var dependentModule in module.DependentModules)
                {
                    deps[dependentModule.Id] = tmp.Clear()
                        .Append("Load ").Append(DependentModuleMetadata.GetLoadType(LoadType.LoadBeforeThis))
                        .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                        .Append(dependentModule.Id)
                        .Append("</a>")
                        .Append(dependentModule.IsOptional ? " (optional)" : string.Empty)
                        .ToString();
                }
                foreach (var dependentModule in module.IncompatibleModules)
                {
                    deps[dependentModule.Id] = tmp.Clear()
                        .Append("Incompatible ")
                        .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                        .Append(dependentModule.Id)
                        .Append("</a>")
                        .ToString();
                }
                foreach (var dependentModule in module.ModulesToLoadAfterThis)
                {
                    deps[dependentModule.Id] = tmp.Clear()
                        .Append("Load ").Append(DependentModuleMetadata.GetLoadType(LoadType.LoadAfterThis))
                        .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                        .Append(dependentModule.Id)
                        .Append("</a>")
                        .ToString();
                }
                foreach (var dependentModule in module.DependentModuleMetadatas)
                {
                    if (dependentModule.IsIncompatible)
                    {
                        deps[dependentModule.Id] = tmp.Clear()
                            .Append("Incompatible ")
                            .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                            .Append(dependentModule.Id)
                            .Append("</a>")
                            .Append(dependentModule.IsOptional ? " (optional)" : string.Empty)
                            .ToString();
                    }
                    else if (dependentModule.LoadType == LoadType.LoadAfterThis)
                    {
                        deps[dependentModule.Id] = tmp.Clear()
                            .Append("Load ").Append(DependentModuleMetadata.GetLoadType(LoadType.LoadAfterThis))
                            .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                            .Append(dependentModule.Id)
                            .Append("</a>")
                            .Append(dependentModule.IsOptional ? " (optional)" : string.Empty)
                            .ToString();
                    }
                    else
                    {
                        deps[dependentModule.Id] = tmp.Clear()
                            .Append("Load ").Append(DependentModuleMetadata.GetLoadType(dependentModule.LoadType))
                            .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependentModule.Id}\").scrollIntoView(false)'>")
                            .Append(dependentModule.Id)
                            .Append("</a>")
                            .Append(dependentModule.IsOptional ? " (optional)" : string.Empty)
                            .ToString();
                    }
                }

                dependenciesBuilder.Clear();
                foreach (var (_, line) in deps)
                {
                    dependenciesBuilder.Append("<li>").Append(line).AppendLine("</li>");
                }
            }

            void AppendSubModules(ModuleInfoExtended module)
            {
                subModulesBuilder.Clear();
                foreach (var subModule in module.SubModules)
                {
                    if (!ModuleInfoHelper.CheckIfSubModuleCanBeLoaded(subModule))
                        continue;

                    assembliesBuilder.Clear();
                    foreach (var assembly in subModule.Assemblies)
                    {
                        assembliesBuilder.Append("<li>").Append(assembly).AppendLine("</li>");
                    }

                    tagsBuilder.Clear();
                    foreach (var (tag, value) in subModule.Tags)
                    {
                        tagsBuilder.Append("<li>").Append(tag).Append(": ").Append(value).AppendLine("</li>");
                    }

                    subModulesBuilder.AppendLine("<li>")
                        .AppendLine(module.IsOfficial ? "<div class=\"submodules-official-container\">" : "<div class=\"submodules-container\">")
                        .Append("<b>").Append(subModule.Name).AppendLine("</b></br>")
                        .Append("Name: ").Append(subModule.Name).AppendLine("</br>")
                        .Append("DLLName: ").Append(subModule.DLLName).AppendLine("</br>")
                        .Append("SubModuleClassType: ").Append(subModule.SubModuleClassType).AppendLine("</br>")
                        .Append(tagsBuilder.Length == 0 ? "" : $"Tags:</br>{NL}")
                        .Append(tagsBuilder.Length == 0 ? "" : $"<ul>{NL}")
                        .Append(tagsBuilder.Length == 0 ? "" : $"{tagsBuilder}{NL}")
                        .Append(tagsBuilder.Length == 0 ? "" : $"</ul>{NL}")
                        .Append(assembliesBuilder.Length == 0 ? "" : $"Assemblies:</br>{NL}")
                        .Append(assembliesBuilder.Length == 0 ? "" : $"<ul>{NL}")
                        .Append(assembliesBuilder.Length == 0 ? "" : $"{assembliesBuilder}{NL}")
                        .Append(assembliesBuilder.Length == 0 ? "" : $"</ul>{NL}")
                        .AppendLine("</div>")
                        .AppendLine("</li>");
                }
            }

            void AppendAdditionalAssemblies(ModuleInfoExtended module)
            {
                additionalAssembliesBuilder.Clear();
                foreach (var externalLoadedAssembly in crashReport.ExternalLoadedAssemblies)
                {
                    if (IsModuleAssembly(module, externalLoadedAssembly))
                    {
                        additionalAssembliesBuilder.Append("<li>")
                            .Append(Path.GetFileName(externalLoadedAssembly.CodeBase))
                            .Append(" (").Append(externalLoadedAssembly.FullName).Append(")")
                            .AppendLine("</li>");
                    }
                }
            }

            moduleBuilder.AppendLine("<ul>");
            foreach (var module in crashReport.LoadedModules)
            {
                AppendDependencies(module);
                AppendSubModules(module);
                AppendAdditionalAssemblies(module);

                moduleBuilder.AppendLine("<li>")
                    .AppendLine(module.IsOfficial ? "<div class=\"modules-official-container\">" : "<div class=\"modules-container\">")
                    .Append($"<b><a href='javascript:;' onclick='showHideById(this, \"{module.Id}\")'>").Append("+ ").Append(module.Name).Append(" (").Append(module.Id).Append(", ").Append(module.Version).Append(")").AppendLine("</a></b>")
                    .AppendLine($"<div id='{module.Id}' style='display: none'>")
                    .Append("Id: ").Append(module.Id).AppendLine("</br>")
                    .Append("Name: ").Append(module.Name).AppendLine("</br>")
                    .Append("Version: ").Append(module.Version).AppendLine("</br>")
                    .Append("Official: ").Append(module.IsOfficial).AppendLine("</br>")
                    .Append("Singleplayer: ").Append(module.IsSingleplayerModule).AppendLine("</br>")
                    .Append("Multiplayer: ").Append(module.IsMultiplayerModule).AppendLine("</br>")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"Dependencies:</br>{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"<ul>{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"{dependenciesBuilder}{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"</ul>{NL}")
                    .Append(string.IsNullOrWhiteSpace(module.Url) ? "" : $"Url: <a href='{module.Url}'>{module.Url}</a></br>{NL}")
                    .Append(subModulesBuilder.Length == 0 ? "" : $"SubModules:</br>{NL}")
                    .Append(subModulesBuilder.Length == 0 ? "" : $"<ul>{NL}")
                    .Append(subModulesBuilder.Length == 0 ? "" : $"{subModulesBuilder}{NL}")
                    .Append(subModulesBuilder.Length == 0 ? "" : $"</ul>{NL}")
                    .Append(additionalAssembliesBuilder.Length == 0 ? "" : $"Additional Assemblies:</br>{NL}")
                    .Append(additionalAssembliesBuilder.Length == 0 ? "" : $"<ul>{NL}")
                    .Append(additionalAssembliesBuilder.Length == 0 ? "" : $"{additionalAssembliesBuilder}{NL}")
                    .Append(additionalAssembliesBuilder.Length == 0 ? "" : $"</ul>{NL}")
                    .AppendLine("</div>")
                    .AppendLine("</div>")
                    .AppendLine("</li>");
            }
            moduleBuilder.AppendLine("</ul>");

            return moduleBuilder.ToString();
        }

        private static string GetAssemblyListHtml(CrashReport crashReport)
        {
            var sb0 = new StringBuilder();

            void AppendAssembly(Assembly assembly, bool isModule = false)
            {
                static string CalculateMD5(string filename)
                {
                    using var md5 = MD5.Create();
                    using var stream = File.OpenRead(filename);
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }

                if (!isModule)
                {
                    foreach (var loadedModule in crashReport.LoadedModules)
                    {
                        if (IsModuleAssembly(loadedModule, assembly))
                        {
                            isModule = true;
                            break;
                        }
                    }
                }
                var isTW = !assembly.IsDynamic && assembly.Location.IndexOf($"Mount & Blade II Bannerlord\\bin\\{TWCommon.ConfigName}\\", StringComparison.InvariantCultureIgnoreCase) >= 0;
                var isSystem = !assembly.IsDynamic && assembly.Location.IndexOf("Windows\\Microsoft.NET\\", StringComparison.InvariantCultureIgnoreCase) >= 0;

                var assemblyName = assembly.GetName();

                sb0.Append(isModule ? "<li class='module_assembly'>" : isTW ? "<li class='tw_assembly'>" : isSystem ? "<li class='sys_assembly'>" : "<li class='unclas_assembly'>")
                    .Append(assemblyName.Name).Append(", ")
                    .Append(assemblyName.Version).Append(", ")
                    .Append(assemblyName.ProcessorArchitecture).Append(", ")
                    .Append(assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.Location) || !File.Exists(assembly.Location) ? "" : $"{CalculateMD5(assembly.Location)}, ")
                    .Append(assembly.IsDynamic ? "DYNAMIC" : string.IsNullOrWhiteSpace(assembly.Location) ? "EMPTY" : !File.Exists(assembly.Location) ? "MISSING" : $"<a href=\"{assembly.Location}\">{assembly.Location}</a>")
                    .AppendLine("</li>");
            }

            sb0.AppendLine("<ul>");
            foreach (var assembly in crashReport.ModuleLoadedAssemblies)
            {
                AppendAssembly(assembly, true);
            }
            foreach (var assembly in crashReport.ExternalLoadedAssemblies)
            {
                AppendAssembly(assembly);
            }
            sb0.AppendLine("</ul>");

            return sb0.ToString();
        }

        private static bool IsModuleAssembly(ModuleInfoExtended loadedModule, Assembly assembly)
        {
            static string PathPrefix() => Path.Combine(TaleWorlds.Library.BasePath.Name, "Modules");
            static string GetPath(string id) => Path.Combine(PathPrefix(), id);

            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.CodeBase))
                return false;

            var modulePath = new Uri(Path.GetFullPath(GetPath(loadedModule.Id)));
            var moduleDirectory = Path.GetFileName(GetPath(loadedModule.Id));

            var assemblyPath = new Uri(assembly.CodeBase);
            var relativePath = modulePath.MakeRelativeUri(assemblyPath);
            return relativePath.OriginalString.StartsWith(moduleDirectory);
        }

        private static string GetHarmonyPatchesListHtml(CrashReport crashReport)
        {
            var harmonyPatchesListBuilder = new StringBuilder();
            var patchesBuilder = new StringBuilder();
            var patchBuilder = new StringBuilder();

            void AppendPatches(string name, IReadOnlyCollection<Patch> patches)
            {
                patchBuilder.Clear();
                foreach (var patch in patches)
                {
                    if (string.Equals(patch.owner, ExceptionHandlerSubSystem.Instance?.Harmony.Id, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    patchBuilder.Append("<li>")
                        .Append("Owner: ").Append(patch.owner).Append("; ")
                        .Append("Namespace: ").Append(patch.PatchMethod.DeclaringType!.FullName).Append(patch.PatchMethod.Name).Append("; ")
                        .Append(patch.index != 0 ? $"Index: {patch.index}; " : "")
                        .Append(patch.priority != 400 ? $"Priority: {patch.priority}; " : "")
                        .Append(patch.before.Length > 0 ? $"Before: {string.Join(", ", patch.before)}; " : "")
                        .Append(patch.after.Length > 0 ? $"After: {string.Join(", ", patch.after)}; " : "")
                        .AppendLine("</li>");
                }

                if (patchBuilder.Length > 0)
                {
                    patchesBuilder.AppendLine("<li>")
                        .AppendLine(name)
                        .AppendLine("<ul>")
                        .AppendLine(patchBuilder.ToString())
                        .AppendLine("</ul>")
                        .AppendLine("</li>");
                }
            }

            harmonyPatchesListBuilder.AppendLine("<ul>");
            foreach (var (originalMethod, patches) in crashReport.LoadedHarmonyPatches)
            {
                patchesBuilder.Clear();

                AppendPatches(nameof(patches.Prefixes), patches.Prefixes);
                AppendPatches(nameof(patches.Postfixes), patches.Postfixes);
                AppendPatches(nameof(patches.Finalizers), patches.Finalizers);
                AppendPatches(nameof(patches.Transpilers), patches.Transpilers);

                if (patchesBuilder.Length > 0)
                {
                    harmonyPatchesListBuilder.AppendLine("<li>")
                        .Append(originalMethod!.DeclaringType!.FullName).Append(".").AppendLine(originalMethod.Name)
                        .AppendLine("<ul>")
                        .AppendLine(patchesBuilder.ToString())
                        .AppendLine("</ul>")
                        .AppendLine("</li>")
                        .AppendLine("<br/>");
                }
            }
            harmonyPatchesListBuilder.AppendLine("</ul>");

            return harmonyPatchesListBuilder.ToString();
        }

        private static string GetLogFilesListHtml(CrashReport crashReport)
        {
            var now = DateTimeOffset.Now;

            var sb = new StringBuilder();

            sb.AppendLine("<ul>");
            foreach (var logSource in ButterLibSubModule.Instance?.GetServiceProvider().GetRequiredService<IEnumerable<ILogSource>>() ?? Enumerable.Empty<ILogSource>())
            {
                sb.Append("<li>").Append("<a>").Append(logSource.Name).Append("</a></br>").Append("<ul>");

                var sbSource = new StringBuilder();
                switch (logSource)
                {
                    case IFileLogSource(_, var path, var sinks) when File.Exists(path):
                    {
                        const string MutexNameSuffix = ".serilog";
                        var mutexName = Path.GetFullPath(path).Replace(Path.DirectorySeparatorChar, ':') + MutexNameSuffix;
                        var mutex = new Mutex(false, mutexName);

                        foreach (var flushableFileSink in sinks)
                            flushableFileSink.FlushToDisk();

                        try
                        {
                            var timeout = TimeSpan.FromSeconds(2);
                            if (!mutex.WaitOne(timeout))
                                break;

                            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            var reader = new ReverseTextReader(stream);
                            while (reader.ReadLine() is { } line)
                            {
                                var idxStart = line.IndexOf('[') + 1;
                                var idxEnd = line.IndexOf(']') - 1;
                                if (!DateTimeOffset.TryParse(line.Substring(idxStart, idxEnd), DateTimeFormatInfo.InvariantInfo, DateTimeStyles.RoundtripKind, out var date))
                                    break;
                                if (date - now > TimeSpan.FromMinutes(60))
                                    break;

                                sbSource.Append("<li>").Append(line).AppendLine("</li>");
                            }
                        }
                        catch (Exception)
                        {
                            sbSource.Clear();
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }

                        break;
                    }
                }

                sb.Append("<ul>").Append(sbSource).AppendLine("</ul>");
                sb.AppendLine("</ul></li>");
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }
    }
}