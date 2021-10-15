using HarmonyLib;

namespace Bannerlord.ButterLib.Implementation.MBSubModuleBaseExtended.Patches
{
    internal static class PatchDebugExtension
    {
        internal static string GetDebugString(this Patch patch)
        {
            return $"\tPatching method: {patch.PatchMethod}\n\tOwner: {patch.PatchMethod.DeclaringType.Assembly.GetName().Name} (HarmonyID: \"{patch.owner}\")\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter: {patch.after}";
        }
    }
}