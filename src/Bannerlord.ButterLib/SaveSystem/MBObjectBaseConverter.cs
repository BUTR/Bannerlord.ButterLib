using Newtonsoft.Json;

using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public sealed class MBObjectBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(MBObjectBase).IsAssignableFrom(objectType);

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is MBObjectBase mbObjectBase)
            {
                serializer.Serialize(writer, mbObjectBase.Id);
                return;
            }

            serializer.Serialize(writer, null);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (serializer.Deserialize<MBGUID?>(reader) is { } mbguid)
            {
                try
                {
                    return MBObjectManager.Instance.GetObject(mbguid);
                }
                catch (Exception e) when(e is MBTypeNotRegisteredException)
                {
                    return null;
                }
            }
            return null;
        }
    }
}