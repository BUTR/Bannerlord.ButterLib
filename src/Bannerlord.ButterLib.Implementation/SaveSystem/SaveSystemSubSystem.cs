using Bannerlord.ButterLib.Implementation.SaveSystem.Patches;

using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.SaveSystem
{
    internal static class SaveSystemSubSystem
    {
        private static readonly Harmony _harmony = new Harmony("Bannerlord.ButterLib.SaveSystem");

        public static void Enable()
        {
            TypeExtensionsPatch.Enable(_harmony); // Adds support for saving many more container types
            //DefinitionContextPatch.Enable(_harmony); // Fixes save corruption & crashes when duplicate types are defined
        }

        public static void Disable()
        {
            TypeExtensionsPatch.Disable(_harmony);
            //DefinitionContextPatch.Disable(_harmony);
        }
    }
}