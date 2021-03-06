﻿using Bannerlord.BUTR.Shared.Helpers;
using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.WinForms;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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

        public static string Build(CrashReport crashReport) => @$"
<html>
  <head>
    <title>Bannerlord Crash Report</title>
    <meta charset='utf-8'>
    <game version='{ApplicationVersionHelper.GameVersionStr()}'>
    <report id='{crashReport.Id}'>
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
    <!--
    <div class='root-container'>
      <h2><a href='javascript:;' class='headers' onclick='showHideById(this, ""log-files"")'>+ Log Files</a></h2>
      <div id='log-files' class='headers-container'>
      {GetLogFilesListHtml(crashReport)}
      </div>
    </div>
    -->
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

        private static string GetModuleListHtml(CrashReport crashReport)
        {
            var moduleBuilder = new StringBuilder();
            var subModulesBuilder = new StringBuilder();
            var assembliesBuilder = new StringBuilder();
            var tagsBuilder = new StringBuilder();
            var additionalAssembliesBuilder = new StringBuilder();
            var dependenciesBuilder = new StringBuilder();
            var dependenciesMetadataBuilder = new StringBuilder();

            void AppendDependencies(ExtendedModuleInfo module)
            {
                dependenciesBuilder.Clear();
                foreach (var dependedModule in module.DependedModules)
                {
                    var dependentModule = crashReport.LoadedModules.Find(m => m.Id == dependedModule.ModuleId);
                    if (dependentModule == null)
                    {
                        dependenciesBuilder.Append("<li>")
                            .Append("MODULE WITH ID: ").Append(dependedModule.ModuleId).Append(" NOT FOUND!")
                            .AppendLine("</li>");
                    }
                    else
                    {
                        dependenciesBuilder.Append("<li>")
                            .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependedModule.ModuleId}\").scrollIntoView(false)'>").Append(dependedModule.ModuleId)
                            .AppendLine("</a></li>");
                    }
                }
            }

            void AppendDependenciesMetadata(ExtendedModuleInfo module)
            {
                dependenciesMetadataBuilder.Clear();
                foreach (var dependedModuleMetadata in module.DependedModuleMetadatas)
                {
                    var dependentModule = crashReport.LoadedModules.Find(m => m.Id == dependedModuleMetadata.Id);
                    if (dependentModule == null && !dependedModuleMetadata.IsOptional)
                    {
                        dependenciesMetadataBuilder.Append("<li>")
                            .Append("ERROR! MODULE WITH ID '").Append(dependedModuleMetadata.Id).Append("' NOT FOUND!")
                            .AppendLine("</li>");
                    }
                    else
                    {
                        dependenciesMetadataBuilder.Append("<li>")
                            .Append("Load ").Append(DependedModuleMetadata.GetLoadType(dependedModuleMetadata.LoadType))
                            .Append($"<a href='javascript:;' onclick='document.getElementById(\"{dependedModuleMetadata.Id}\").scrollIntoView(false)'>")
                            .Append(dependedModuleMetadata.Id)
                            .AppendLine("</a></li>");
                    }
                }
            }

            void AppendSubModules(ExtendedModuleInfo module)
            {
                subModulesBuilder.Clear();
                foreach (var subModule in module.ExtendedSubModules)
                {
                    if (!subModule.IsLoadable)
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
                        //.Append("Verified: ").Append(subModule.IsVerifiedDLL).AppendLine("</br>")
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

            void AppendAdditionalAssemblies(ExtendedModuleInfo module)
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
                AppendDependenciesMetadata(module);
                AppendSubModules(module);
                AppendAdditionalAssemblies(module);

                moduleBuilder.AppendLine("<li>")
                    .AppendLine(module.IsOfficial ? "<div class=\"modules-official-container\">" : "<div class=\"modules-container\">")
                    .Append($"<b><a href='javascript:;' onclick='showHideById(this, \"{module.Id}\")'>").Append("+ ").Append(module.Name).Append(" (").Append(module.Id).Append(", ").Append(module.Version).Append(")").AppendLine("</a></b>")
                    .AppendLine($"<div id='{module.Id}' style='display: none'>")
                    .Append("Id: ").Append(module.Id).AppendLine("</br>")
                    .Append("Name: ").Append(module.Name).AppendLine("</br>")
                    .Append("Version: ").Append(module.Version.ToString()).AppendLine("</br>")
                    .Append("Official: ").Append(module.IsOfficial.ToString()).AppendLine("</br>")
                    .Append("Singleplayer: ").Append(module.IsSingleplayerModule.ToString()).AppendLine("</br>")
                    .Append("Multiplayer: ").Append(module.IsMultiplayerModule.ToString()).AppendLine("</br>")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"Dependencies:</br>{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"<ul>{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"{dependenciesBuilder}{NL}")
                    .Append(dependenciesBuilder.Length == 0 ? "" : $"</ul>{NL}")
                    .Append(dependenciesMetadataBuilder.Length == 0 ? "" : $"Dependency Metadatas:</br>{NL}")
                    .Append(dependenciesMetadataBuilder.Length == 0 ? "" : $"<ul>{NL}")
                    .Append(dependenciesMetadataBuilder.Length == 0 ? "" : $"{dependenciesMetadataBuilder}{NL}")
                    .Append(dependenciesMetadataBuilder.Length == 0 ? "" : $"</ul>{NL}")
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

        private static bool IsModuleAssembly(ExtendedModuleInfo loadedModule, Assembly assembly)
        {
            if (assembly.IsDynamic || string.IsNullOrWhiteSpace(assembly.CodeBase))
                return false;

            var modulePath = new Uri(Path.GetFullPath(loadedModule.Folder));
            var moduleDirectory = Path.GetFileName(loadedModule.Folder);

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
                    if (string.Equals(patch.owner, ExceptionHandlerSubSystem.Harmony.Id, StringComparison.InvariantCultureIgnoreCase))
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
            return "";
        }
    }
}