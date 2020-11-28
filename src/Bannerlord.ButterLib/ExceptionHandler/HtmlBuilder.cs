using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.Common.Helpers;
using Bannerlord.ButterLib.ExceptionHandler.WinForms;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var html = Build(crashReport);
            using var form = new HtmlCrashReportForm(html);
            form.ShowDialog();
        }

        public static string Build(CrashReport crashReport) => @$"
<html>
  <head>
    <meta charset='utf-8'>
    <game version='{ApplicationVersionUtils.GameVersionStr()}'>
  </head>
  <body>
     <table style='width: 100%;'>
      <tbody>
        <tr>
          <td style='width: 80%;'>
            <p>
              <b>Bannerlord has encountered a problem and will close itself.</b>
              <br/>
              This is a community Crash Report. Please save it and use it for reporting the error.
              <br/>
              Most likely this error was caused by a custom installed module.
              <br/>
              <br/>
              If you were in the middle of something, the progress might be lost.
              <br/>
            </p>
          </td>
          <td>
            <select style='float:right; margin-left:10px;' class=""input"" onchange=""changeFontSize(this);"">
              <option value=""1.0em"" selected=""selected"">Standard</option>
			  <option value=""0.9em"">Medium</option>
              <option value=""0.8em"">Small</option>
            </select>
          </td>
        </tr>
      </tbody>
    </table>
    <h2><a href='javascript:;' onclick='showHideById(this, ""exception"")'>+ Exception</a></h2>
    <div id='exception' style='font-family: ""Consolas""; font-size: small; display: none'>
    {GetRecursiveExceptionHtml(crashReport.Exception)}
    </div>
    <h2><a href='javascript:;' onclick='showHideById(this, ""installed-modules"")'>+ Installed Modules</a></h2>
    <div id='installed-modules' style='font-family: ""Consolas""; font-size: small; display: none'>
    {GetModuleListHtml(crashReport)}
    </div>
    <h2><a href='javascript:;' onclick='showHideById(this, ""assemblies"")'>+ Assemblies</a></h2>
    <div id='assemblies' style='font-family: ""Consolas""; font-size: small; display: none'>
    <label>Hide: </label>
    <label><input type='checkbox' onclick='showHideByClassName(this, ""tw_assembly"")'> TaleWorlds</label>
    <label><input type='checkbox' onclick='showHideByClassName(this, ""sys_assembly"")'> System</label>
    <label><input type='checkbox' onclick='showHideByClassName(this, ""module_assembly"")'> Modules</label>
    <label><input type='checkbox' onclick='showHideByClassName(this, ""unclas_assembly"")'> Unclassified</label>
    {GetAssemblyListHtml(crashReport)}
    </div>
    <h2><a href='javascript:;' onclick='showHideById(this, ""harmony-patches"")'>+ Harmony Patches</a></h2>
    <div id='harmony-patches' style='font-family: ""Consolas""; font-size: small; display: none'>
    {GetHarmonyPatchesListHtml(crashReport)}
    </div>
    <script>
      function showHideById(element, id) {{
          if (document.getElementById(id).style.display === 'none') {{
              document.getElementById(id).style.display = 'block';
              element.innerHTML = element.innerHTML.replace('+', '-');
          }} else {{
              document.getElementById(id).style.display = 'none';
              element.innerHTML = element.innerHTML.replace('-', '+');
          }}
      }}
      function showHideByClassName(element, className) {{
          var list = document.getElementsByClassName(className)
          for (var i = 0; i < list.length; i++) {{
              list[i].style.display = (element.checked) ? 'none' : 'list-item';
          }}
      }}
       function changeFontSize(fontSize) {{
          document.getElementById(""exception"").style.fontSize = fontSize.value;
          document.getElementById(""installed-modules"").style.fontSize = fontSize.value;
          document.getElementById(""assemblies"").style.fontSize = fontSize.value;
          document.getElementById(""harmony-patches"").style.fontSize = fontSize.value;
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
            var sb0 = new StringBuilder();
            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();

            sb0.AppendLine("<ul>");
            foreach (var module in crashReport.LoadedModules)
            {
                sb1.Clear();
                sb1.AppendLine("<ul>");
                foreach (var subModule in module.SubModules)
                {
                    sb2.Clear();
                    sb2.AppendLine("<ul>");
                    foreach (var assembly in subModule.Assemblies)
                    {
                        sb2.Append("<li>").Append(assembly).AppendLine("</li>");
                    }
                    sb2.AppendLine("</ul>");

                    sb1.AppendLine("<li>")
                        .Append(subModule.Name).Append(" (").Append(subModule.DLLName).AppendLine(")")
                        .AppendLine(sb2.ToString())
                        .AppendLine("</li>");
                }
                sb1.AppendLine("</ul>");

                sb0.AppendLine("<li>")
                    .Append(string.IsNullOrWhiteSpace(module.Url) ? "<a>" : $"<a href='{module.Url}'>").Append(module.Name).Append(" (").Append(module.Id).Append(", ").Append(module.Version).Append(")").AppendLine("</a>")
                    .AppendLine(sb1.ToString())
                    .AppendLine("</li>");
            }
            sb0.AppendLine("</ul>");

            return sb0.ToString();
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

        private static string GetHarmonyPatchesListHtml(CrashReport crashReport)
        {
            var sb0 = new StringBuilder();
            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();

            void AppendPatches(string name, IReadOnlyCollection<Patch> patches)
            {
                sb2.Clear();
                foreach (var patch in patches)
                {
                    sb2.Append("<li>")
                        .Append("Owner: ").Append(patch.owner).Append("; ")
                        .Append("Namespace: ").Append(patch.PatchMethod.DeclaringType!.FullName).Append(patch.PatchMethod.Name).Append("; ")
                        .Append(patch.index != 0 ? $"Index: {patch.index}; " : "")
                        .Append(patch.priority != 400 ? $"Priority: {patch.priority}; " : "")
                        .Append(patch.before.Length > 0 ? $"Before: {string.Join(", ", patch.before)}; " : "")
                        .Append(patch.before.Length > 0 ? $"After: {string.Join(", ", patch.after)}; " : "")
                        .AppendLine("</li>");
                }

                if (patches.Count > 0)
                {
                    sb1.AppendLine("<li>")
                        .AppendLine(name)
                        .AppendLine("<ul>")
                        .AppendLine(sb2.ToString())
                        .AppendLine("</ul>")
                        .AppendLine("</li>");
                }
            }

            sb0.AppendLine("<ul>");
            foreach (var (originalMethod, patches) in crashReport.LoadedHarmonyPatches)
            {
                sb1.Clear();

                AppendPatches(nameof(patches.Prefixes), patches.Prefixes);
                AppendPatches(nameof(patches.Postfixes), patches.Postfixes);
                AppendPatches(nameof(patches.Finalizers), patches.Finalizers);
                AppendPatches(nameof(patches.Transpilers), patches.Transpilers);

                sb0.AppendLine("<li>")
                    .Append(originalMethod!.DeclaringType!.FullName).Append(".").AppendLine(originalMethod.Name)
                    .AppendLine("<ul>")
                    .AppendLine(sb1.ToString())
                    .AppendLine("</ul>")
                    .AppendLine("</li>")
                    .AppendLine("<br/>");
            }
            sb0.AppendLine("</ul>");

            return sb0.ToString();
        }
    }
}