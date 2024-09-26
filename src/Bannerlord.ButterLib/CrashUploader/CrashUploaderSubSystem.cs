using Bannerlord.ButterLib.Options;
using Bannerlord.ButterLib.SubSystems;

namespace Bannerlord.ButterLib.CrashUploader;

internal sealed class CrashUploaderSubSystem : ISubSystem
{
    public static CrashUploaderSubSystem? Instance { get; private set; }

    public string Id => "CrashUploader";
    public string Name => "{=UsLlrwMTjJ}Crash Uploader";
    public string Description => "{=hjeoN9NwZm}Uploads the crash reports to BUTR for an easy file hosting.";
    public bool IsEnabled { get; private set; }
    public bool CanBeDisabled => true;
    public bool CanBeSwitchedAtRuntime => true;

    private bool _wasInitialized;

    public CrashUploaderSubSystem()
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
    }

    public void Disable()
    {
        if (!IsEnabled) return;
        IsEnabled = false;
    }
}