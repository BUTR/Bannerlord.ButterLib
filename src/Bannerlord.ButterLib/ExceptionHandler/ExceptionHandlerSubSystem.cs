using Bannerlord.BLSE;
using Bannerlord.ButterLib.Options;
using Bannerlord.ButterLib.SubSystems;
using Bannerlord.ButterLib.SubSystems.Settings;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;

namespace Bannerlord.ButterLib.ExceptionHandler;

[BLSEInterceptor]
internal sealed class ExceptionHandlerSubSystem : ISubSystem, ISubSystemSettings<ExceptionHandlerSubSystem>
{
    private delegate void DetachWatchdogDelegate();
    private static readonly DetachWatchdogDelegate? DetachWatchdog =
        AccessTools2.GetDelegate<DetachWatchdogDelegate>("TaleWorlds.Engine.Utilities:DetachWatchdog");

    public static ExceptionHandlerSubSystem? Instance { get; private set; }

    internal readonly Harmony Harmony = new("Bannerlord.ButterLib.ExceptionHandler.BEW");

    public string Id => "ExceptionHandler";
    public string Name => "{=ZypQtNNcVN}Exception Handler";
    public string Description => "{=UreeIeLQYS}Captures game crashes and creates reports out of them.";
    public bool IsEnabled { get; private set; }
    public bool CanBeDisabled => true;
    public bool CanBeSwitchedAtRuntime => true;

    private bool _disableWhenDebuggerIsAttached = true;
    public bool DisableWhenDebuggerIsAttached
    {
        get => _disableWhenDebuggerIsAttached;
        private set
        {
            if (_disableWhenDebuggerIsAttached == value) return;

            _disableWhenDebuggerIsAttached = value;

            if (BEWPatch.IsDebuggerAttached())
            {
                if (_disableWhenDebuggerIsAttached)
                {
                    Disable();
                }
                else
                {
                    SubscribeToUnhandledException();
                }
            }
            else
            {
                SubscribeToUnhandledException();
            }
        }
    }

    private bool _wasInitialized;

    /// <inheritdoc />
    public IReadOnlyCollection<SubSystemSettingsDeclaration<ExceptionHandlerSubSystem>> Declarations { get; } =
    [
        new SubSystemSettingsPropertyBool<ExceptionHandlerSubSystem>(
            "{=B7bfrDNzIk} Disable when Debugger is Attached",
            "{=r3ktQzFMRz} Stops the Exception Handler when a debugger is attached.",
            x => x.DisableWhenDebuggerIsAttached),
    ];


    public ExceptionHandlerSubSystem()
    {
        Instance = this;
    }

    public void Enable()
    {
        if (!_wasInitialized)
        {
            _wasInitialized = true;
            var isEnabledViaSettings = SettingsProvider.PopulateSubSystemSettings(this) ?? true;
            if (!isEnabledViaSettings) return;
        }

        if (IsEnabled) return;
        IsEnabled = true;

        if (!BEWPatch.IsDebuggerAttached())
            SubscribeToUnhandledException();
        else if (_disableWhenDebuggerIsAttached)
            return;

        if (!_wasButrLoaderInterceptorCalled)
        {
            BEWPatch.Enable(Harmony);
            DetachWatchdog?.Invoke();
        }
    }

    public void Disable()
    {
        if (!IsEnabled) return;
        IsEnabled = false;

        UnsubscribeToUnhandledException();

        if (BUTR.Shared.Helpers.ModuleInfoHelper.GetLoadedModules().Any(m => string.Equals(m.Id, "BetterExceptionWindow", StringComparison.InvariantCultureIgnoreCase)))
        {
            BEWPatch.Disable(Harmony);
        }
    }

    private static bool _isSubscribedToUnhandledException;
    private static bool _wasButrLoaderInterceptorCalled;

    private static void SubscribeToUnhandledException()
    {
        if (!_isSubscribedToUnhandledException && !_wasButrLoaderInterceptorCalled)
        {
            _isSubscribedToUnhandledException = true;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
    }
    private static void UnsubscribeToUnhandledException()
    {
        if (_isSubscribedToUnhandledException && !_wasButrLoaderInterceptorCalled)
        {
            _isSubscribedToUnhandledException = false;
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomain_UnhandledException;
        }
    }

    [HandleProcessCorruptedStateExceptions, SecurityCritical]
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            ExceptionReporter.Show(exception);
        }
    }

    // BLSE Duck typed method
    private static void OnInitializeSubModulesPrefix()
    {
        _wasButrLoaderInterceptorCalled = true;
    }
}