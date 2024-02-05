using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using System.Collections.Generic;
using System.Linq;

using TaleWorlds.MountAndBlade.Options;

namespace Bannerlord.ButterLib.Implementation.HotKeys.Patches;

internal sealed class OptionsProviderPatches
{
    internal static bool Enable(Harmony harmony)
    {
            return harmony.TryPatch(AccessTools2.Method(typeof(OptionsProvider), nameof(OptionsProvider.GetGameKeyCategoriesList)),
                postfix: AccessTools2.Method(typeof(OptionsProviderPatches), nameof(GetGameKeyCategoriesListPostfix)));
        }

    internal static bool Disable(Harmony harmony)
    {
            harmony.Unpatch(AccessTools2.Method(typeof(OptionsProvider), nameof(OptionsProvider.GetGameKeyCategoriesList)),
                AccessTools2.Method(typeof(OptionsProviderPatches), nameof(GetGameKeyCategoriesListPostfix)));

            return true;
        }

    private static IEnumerable<string> GetGameKeyCategoriesListPostfix(IEnumerable<string> values)
    {
            return values.Concat(HotKeyManagerImplementation.GlobalContainerStorage.Select(x => x.GameKeyCategoryId)).Distinct();
        }
}