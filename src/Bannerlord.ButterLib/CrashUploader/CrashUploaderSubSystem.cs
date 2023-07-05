using Bannerlord.ButterLib.SubSystems;

namespace Bannerlord.ButterLib.CrashUploader
{
    internal sealed class CrashUploaderSubSystem : ISubSystem
    {
        public static CrashUploaderSubSystem? Instance { get; private set; }

        public string Id => "CrashUploader";
        public string Name => "{=UsLlrwMTjJ}Crash Uploader";
        public string Description => "{=hjeoN9NwZm}Uploads the crash reports to BUTR for an easy file hosting.";
        public bool IsEnabled { get; private set; }
        public bool CanBeDisabled => true;
        public bool CanBeSwitchedAtRuntime => true;


        public CrashUploaderSubSystem()
        {
            Instance = this;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}