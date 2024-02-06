using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.ButterLib.ObjectSystem;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem;

public sealed class MBObjectBaseConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => typeof(MBObjectBase).IsAssignableFrom(objectType);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is MBObjectBase mbObject)
        {
            var keeper = ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IMBObjectKeeper>();
            keeper?.Keep(mbObject);

            serializer.Serialize(writer, mbObject.Id);
            return;
        }

        serializer.Serialize(writer, null);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (serializer.Deserialize<MBGUID?>(reader) is { } mbguid)
        {
            var finder = ButterLibSubModule.Instance?.GetServiceProvider()?.GetService<IMBObjectFinder>();
            return finder?.Find(mbguid, objectType);
        }
        return null;
    }
}