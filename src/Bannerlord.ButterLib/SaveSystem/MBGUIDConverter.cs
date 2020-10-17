using Newtonsoft.Json;

using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public class MBGUIDConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(MBGUID);

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is MBGUID mbguid)
            {
                serializer.Serialize(writer, mbguid.InternalValue);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (serializer.Deserialize<uint?>(reader) is { } id)
            {
                return new MBGUID(id);
            }
            return null;
        }
    }
}