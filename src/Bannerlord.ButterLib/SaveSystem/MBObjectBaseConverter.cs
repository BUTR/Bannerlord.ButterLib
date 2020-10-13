using Newtonsoft.Json;

using System;
using System.Reflection;

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
                if (MBObjectManager.Instance.FindRegisteredClassPrefix(value.GetType()) is { } classPrefix)
                {
                    // Type is registered
                    serializer.Serialize(writer, classPrefix);
                    serializer.Serialize(writer, mbObjectBase.StringId);
                }
                else
                {
                    // Type is a custom one
                    serializer.Serialize(writer, null);
                    serializer.Converters.Remove(this);
                    serializer.Serialize(writer, value);
                    serializer.Converters.Add(this);
                }
            }
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (serializer.Deserialize<string>(reader) is { } classPrefix)
            {
                // Type is registered
                reader.Read();
                if (serializer.Deserialize<string>(reader) is { } stringId)
                {
                    //reader.Read();
                    var method = typeof(MBObjectManager).GetMethod("GetObject", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(string) }, null);
                    var genericMethod = method?.MakeGenericMethod(objectType);
                    return genericMethod?.Invoke(MBObjectManager.Instance, new object[] { stringId });
                }
                return null;
            }
            else
            {
                // Type is a custom one
                reader.Read();
                serializer.Converters.Remove(this);
                var result = serializer.Deserialize(reader, objectType);
                //reader.Read();
                serializer.Converters.Add(this);
                return result;
            }
        }
    }
}