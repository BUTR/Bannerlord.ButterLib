using HarmonyLib.BUTR.Extensions;

using Newtonsoft.Json.Serialization;

using System;
using System.Collections.Generic;

namespace Bannerlord.ButterLib.SaveSystem;

public class TaleWorldsContractResolverBase : DefaultContractResolver
{
    protected delegate bool IsContainerDelegate(Type type);
    protected static readonly IsContainerDelegate? _isContainerDelegate =
        AccessTools2.GetDelegate<IsContainerDelegate>("TaleWorlds.SaveSystem.TypeExtensions:IsContainer", [typeof(Type)]);

    protected static bool IsContainerFallback(Type type)
    {
        if (type is { IsGenericType: true, IsGenericTypeDefinition: false })
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Dictionary<,>))
                return true;
            if (genericTypeDefinition == typeof(List<>))
                return true;
            if (genericTypeDefinition == typeof(Queue<>))
                return true;
        }
        else if (type.IsArray)
        {
            return true;
        }

        return false;
    }
}