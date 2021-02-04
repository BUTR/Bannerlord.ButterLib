#if e143 || e150 || e151 || e152 || e153
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
    internal sealed class ClanInitializeClanPatch
    {
        private static ILogger _logger = default!;

        // Application:

        internal static void Enable(Harmony harmony)
        {
            var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
            _logger = provider?.GetRequiredService<ILogger<ClanInitializeClanPatch>>() ?? NullLogger<ClanInitializeClanPatch>.Instance;

            if (InitializeClanTargetMethod is null)
                _logger.LogError("InitializeClanTargetMethod is null");
            if (InitializeClanPatchMethod is null)
                _logger.LogError("InitializeClanPatchMethod is null");

            if (InitializeClanTargetMethod is null || InitializeClanPatchMethod is null)
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
            if (__instance is null)
            {
                _logger.LogError("InitializeClanPostfix: __instance is null");
                return;
            }

            if (__instance == Clan.PlayerClan)
            {
                if (CampaignIdentifierEvents.Instance is null)
                {
                    _logger.LogError("InitializeClanPostfix: CampaignIdentifierEvents.Instance is null");
                    return;
                }

                CampaignIdentifierEvents.Instance.OnDescriptorRelatedDataChanged();
            }
        }
    }
}
#endif