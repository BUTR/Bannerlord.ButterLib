using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System.Reflection;

using TaleWorlds.CampaignSystem;

using static HarmonyLib.AccessTools;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.Patches
{
    internal class ClanInitializeClanPatch
    {
        private static ILogger _logger = default!;

        // Application:

        internal static void Apply(Harmony harmony)
        {
            _logger = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<ClanInitializeClanPatch>>() ??
                      NullLogger<ClanInitializeClanPatch>.Instance;

            if (InitializeClanTargetMethod == null)
                _logger.LogError("InitializeClanTargetMethod is null");
            if (InitializeClanPatchMethod == null)
                _logger.LogError("InitializeClanPatchMethod is null");

            if (InitializeClanTargetMethod == null || InitializeClanPatchMethod == null)
            {
                return;
            }

            harmony.Patch(InitializeClanTargetMethod, postfix: new HarmonyMethod(InitializeClanPatchMethod));
        }

        // Target and patch methods:

        private static readonly MethodInfo? InitializeClanTargetMethod =
            Method(typeof(Clan), "InitializeClan");

        private static readonly MethodInfo? InitializeClanPatchMethod =
            Method(typeof(ClanInitializeClanPatch), nameof(InitializeClanPostfix));

        public static void InitializeClanPostfix(Clan? __instance)
        {
            if (__instance == null)
            {
                _logger.LogError("InitializeClanPostfix: __instance is null");
                return;
            }

            if (__instance == Clan.PlayerClan)
            {
                if (CampaignIdentifierEvents.Instance == null)
                {
                    _logger.LogError("InitializeClanPostfix: CampaignIdentifierEvents.Instance is null");
                    return;
                }

                CampaignIdentifierEvents.Instance.OnDescriptorRelatedDataChanged();
            }
        }
    }
}