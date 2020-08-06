using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.CampaignIdentifier.Helpers
{
    internal static class FieldAccessHelper
    {
        internal static readonly FieldRef<TextObject, string> TextObjectValueByRef = FieldRefAccess<TextObject, string>("Value");
        internal static readonly FieldRef<Clan, Settlement> ClanHomeSettlementByRef = FieldRefAccess<Clan, Settlement>("HomeSettlement");
    }
}
