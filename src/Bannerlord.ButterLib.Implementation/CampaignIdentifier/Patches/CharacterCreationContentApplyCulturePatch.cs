using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Helpers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using StoryMode.CharacterCreationSystem;

using System.Reflection;

using TaleWorlds.CampaignSystem;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches
{
    internal sealed class CharacterCreationContentApplyCulturePatch
    {
        private static ILogger _logger = default!;

        // Application:

        internal static void Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();

            _logger = provider?.GetRequiredService<ILogger<CharacterCreationContentApplyCulturePatch>>()
                      ?? NullLogger<CharacterCreationContentApplyCulturePatch>.Instance;

            if (ApplyCultureTargetMethod is null)
                _logger.LogError("ApplyCultureTargetMethod is null");
            if (ApplyCulturePatchMethod is null)
                _logger.LogError("ApplyCulturePatchMethod is null");

            if (ApplyCultureTargetMethod is null || ApplyCulturePatchMethod is null)
            {
                return;
            }

            harmony.Patch(ApplyCultureTargetMethod, postfix: new HarmonyMethod(ApplyCulturePatchMethod));
        }

        // Target and patch methods:

        private static readonly MethodInfo? ApplyCultureTargetMethod =
            Method(typeof(CharacterCreationContent), "ApplyCulture");

        private static readonly MethodInfo? ApplyCulturePatchMethod =
            Method(typeof(CharacterCreationContentApplyCulturePatch), nameof(ApplyCulturePostfix));

        // Necessary reflection:

        private static readonly FieldRef<Clan, Settlement>? ClanHomeSettlementByRef =
            FieldRefAccess<Clan, Settlement>("_home");

        public static void ApplyCulturePostfix()
        {
            if (ClanHomeSettlementByRef is null)
            {
                _logger.LogError("ApplyCulturePostfix: ClanHomeSettlementByRef is null");
                return;
            }

            //Assign player a random town from chosen culture as a born settlement
            ref var clanSettlement = ref ClanHomeSettlementByRef.Invoke(Clan.PlayerClan);
            clanSettlement = SettlementHelper.FindRandomSettlement(s => s.Culture == Hero.MainHero.Culture && s.IsTown);

            foreach (var hero in Clan.PlayerClan.Heroes)
                hero.UpdateHomeSettlement();

            Hero.MainHero.BornSettlement = Clan.PlayerClan.HomeSettlement;
        }
    }
}