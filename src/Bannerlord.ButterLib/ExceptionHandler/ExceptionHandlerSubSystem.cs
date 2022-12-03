﻿using Bannerlord.ButterLib.SubSystems;
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

        private bool _disableWhenDebuggerIsAttached = true;
        public bool DisableWhenDebuggerIsAttached
        {
            get => _disableWhenDebuggerIsAttached;
            private set
            {
                if (_disableWhenDebuggerIsAttached != value)
                {
                    _disableWhenDebuggerIsAttached = value;

                    if (BEWPatch.IsDebuggerAttached())
                    {
                        if (_disableWhenDebuggerIsAttached)
                            UnsubscribeToUnhandledException();
                        else
                            SubscribeToUnhandledException();
                    }
                    else
                    {
                        SubscribeToUnhandledException();
                    }
                }
            }
        }

        private bool _catchAutoGenExceptions = false;
        public bool CatchAutoGenExceptions
        {
            get => _catchAutoGenExceptions;
            private set
            {
                if (_catchAutoGenExceptions != value)
                {
                    _catchAutoGenExceptions = value;

                    /*
                    if (_catchAutoGenExceptions)
                    {
                        if (IsEnabled)
                        {
                            BEWPatch.Disable(Harmony);
                            BEWPatch.EnableWithDebug(Harmony);
                        }
                    }
                    else
                    {
                        if (IsEnabled)
                        {
                            BEWPatch.DisableWithDebug(Harmony);
                            BEWPatch.Enable(Harmony);
                        }
                    }
                    */
                }
            }
        }

        /// <inheritdoc />
        public IReadOnlyCollection<SubSystemSettingsDeclaration<ExceptionHandlerSubSystem>> Declarations { get; } = new SubSystemSettingsDeclaration<ExceptionHandlerSubSystem>[]
        {
            new SubSystemSettingsPropertyBool<ExceptionHandlerSubSystem>(
                "{=B7bfrDNzIk} Disable when Debugger is Attached",
                "{=r3ktQzFMRz} Stops the Exception Handler when a debugger is attached.",
                x => x.DisableWhenDebuggerIsAttached),
            new SubSystemSettingsPropertyBool<ExceptionHandlerSubSystem>(
                "{=B7bfrDNzIk} Catch AutoGenerated Code Exceptions (Lower Performance)",
                "{=r3ktQzFMRz} Catch every Native->Managed call. Should catch every exception not catched the standard way. Might decrease the overall performance a bit.",
                x => x.CatchAutoGenExceptions),
        };


        public ExceptionHandlerSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;

            if (!BEWPatch.IsDebuggerAttached())
                SubscribeToUnhandledException();

            BEWPatch.Enable(Harmony);
        }

        public void Disable()
        {
            IsEnabled = false;

            UnsubscribeToUnhandledException();

            if (BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
            {
                BEWPatch.Disable(Harmony);
            }
        }

        private static bool _isSubscribedToUnhandledException;
        private static void SubscribeToUnhandledException()
        {
            if (!_isSubscribedToUnhandledException)
            {
                _isSubscribedToUnhandledException = true;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            }
        }
        private static void UnsubscribeToUnhandledException()
        {
            if (_isSubscribedToUnhandledException)
            {
                _isSubscribedToUnhandledException = false;
                AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
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