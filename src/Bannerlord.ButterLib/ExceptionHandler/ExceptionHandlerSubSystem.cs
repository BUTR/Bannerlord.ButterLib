using Bannerlord.ButterLib.SubSystems;

using HarmonyLib;

using System;
using System.Linq;

namespace Bannerlord.ButterLib.ExceptionHandler
{
    internal sealed class ExceptionHandlerSubSystem : ISubSystem
    {
        public static ExceptionHandlerSubSystem? Instance { get; private set; }

        internal readonly Harmony Harmony = new("Bannerlord.ButterLib.ExceptionHandler.BEW");

        public string Id => "ExceptionHandler";
        public string Description => "Captures game crashes and creates reports out of them.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => true;


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