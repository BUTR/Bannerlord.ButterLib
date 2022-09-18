using Bannerlord.ButterLib.SubSystems;
using Bannerlord.ButterLib.SubSystems.Settings;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class ExceptionHandlerSubSystem : ISubSystem, ISubSystemSettings<ExceptionHandlerSubSystem>
    {
        public static ExceptionHandlerSubSystem? Instance { get; private set; }

        internal readonly Harmony Harmony = new("Bannerlord.ButterLib.ExceptionHandler.BEW");

        public string Id => "ExceptionHandler";
        public string Description => "Captures game crashes and creates reports out of them.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => true;

        public bool DisableWhenDebuggerIsAttached { get; private set; } = true;

        /// <inheritdoc />
        public IReadOnlyCollection<SubSystemSettingsDeclaration<ExceptionHandlerSubSystem>> Declarations { get; } = new SubSystemSettingsDeclaration<ExceptionHandlerSubSystem>[]
        {
            new SubSystemSettingsPropertyBool<ExceptionHandlerSubSystem>("{=B7bfrDNzIk}", "{=r3ktQzFMRz}", x => x.DisableWhenDebuggerIsAttached)
        };


        public ExceptionHandlerSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            BEWPatch.Enable(Harmony);
        }

        public void Disable()
        {
            IsEnabled = false;

            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;

            if (BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                BEWPatch.Disable(Harmony);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                ExceptionReporter.Show(exception);
            }
        }
    }
}