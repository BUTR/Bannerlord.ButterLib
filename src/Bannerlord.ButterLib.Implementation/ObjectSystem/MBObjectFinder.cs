using Bannerlord.ButterLib.ObjectSystem;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

using AccessTools2 = HarmonyLib.BUTR.Extensions.AccessTools2;

namespace Bannerlord.ButterLib.Implementation.ObjectSystem;

internal class MBObjectFinder : IMBObjectFinder
{
    private static readonly AccessTools.FieldRef<CampaignObjectManager, object?[]>? CampaignObjectTypeObjects =
        AccessTools2.FieldRefAccess<CampaignObjectManager, object[]>("_objects");
    private static readonly Type? ICampaignObjectTypeType =
        AccessTools2.TypeByName("TaleWorlds.CampaignSystem.CampaignObjectManager+ICampaignObjectType");
    private static readonly MethodInfo? ObjectClassGetter =
        AccessTools2.PropertyGetter(ICampaignObjectTypeType!, "ObjectClass");

    private static MBObjectBase? FindCampaignObjectManager(MBGUID id, Type type)
    {
        // Not sure this piece if code ever worked, but keeping it just in case of edgecases
        foreach (var cot in CampaignObjectTypeObjects?.Invoke(Campaign.Current.CampaignObjectManager) ?? [])
        {
            if (cot is null) continue;
            
            if (ObjectClassGetter?.Invoke(cot, []) is Type objType && objType == type && cot is IEnumerable<MBObjectBase> en && en.FirstOrDefault(o => o.Id == id) is { } result)
            {
                return result;
            }
        }

        try
        {
            return MBObjectManager.Instance.GetObject(id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public MBObjectBase? Find(MBGUID id, Type? type = null)
    {
        return FindCampaignObjectManager(id, type ?? typeof(MBObjectBase));
    }
}