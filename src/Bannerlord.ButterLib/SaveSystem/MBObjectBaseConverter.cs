using Newtonsoft.Json;

using System;

using TaleWorlds.ObjectSystem;

namespace Bannerlord.ButterLib.SaveSystem
{
    public class MBObjectBaseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(MBObjectBase).IsAssignableFrom(objectType);

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is MBObjectBase mbObjectBase)
            {
                serializer.Serialize(writer, mbObjectBase.Id.InternalValue);
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //reader.Read();
            if (serializer.Deserialize<uint?>(reader) is { } id)
            {
                return MBObjectManager.Instance.GetObject(new MBGUID(id));
            }
            return null;
        }
    }
}