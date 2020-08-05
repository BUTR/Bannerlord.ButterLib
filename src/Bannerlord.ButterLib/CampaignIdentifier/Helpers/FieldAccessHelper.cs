using TaleWorlds.Localization;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.CampaignIdentifier.Helpers
{
    internal static class FieldAccessHelper
    {
        internal static readonly FieldRef<TextObject, string> TextObjectValueByRef = FieldRefAccess<TextObject, string>("Value");
    }
}