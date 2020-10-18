using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bannerlord.ButterLib.SaveSystem
{
    public sealed class DictionaryToArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(IDictionary).IsAssignableFrom(objectType) || TypeImplementsGenericInterface(objectType, typeof(IDictionary<,>));

        private static bool TypeImplementsGenericInterface(Type concreteType, Type interfaceType) => concreteType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IDictionary dictionary)
            {
                var genericArguments = value.GetType().GetGenericArguments();

                writer.WriteStartArray();
                foreach (DictionaryEntry dictionaryEntry in dictionary)
                {
                    // TODO: Return to this
                    if (dictionaryEntry.Key != null)
                    {
                        serializer.Serialize(writer, dictionaryEntry.Key, genericArguments[0]);
                        serializer.Serialize(writer, dictionaryEntry.Value, genericArguments[1]);
                    }
                }
                writer.WriteEndArray();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!(existingValue is IDictionary dict))
            {
                var contract = serializer.ContractResolver.ResolveContract(objectType);
                dict = (IDictionary) contract.DefaultCreator();
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();
                var genericArguments = objectType.GetGenericArguments();
                do
                {
                    var key = serializer.Deserialize(reader, genericArguments[0]);
                    reader.Read();
                    var value = serializer.Deserialize(reader, genericArguments[1]);
                    reader.Read();
                    // TODO: Return to this
                    if (key != null)
                    {
                        if (dict.Contains(key))
                            dict[key] = value;
                        else
                            dict.Add(key, value);
                    }
                } while (reader.TokenType != JsonToken.EndArray);
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                // Using "Populate()" avoids infinite recursion.
                // https://github.com/JamesNK/Newtonsoft.Json/blob/ee170dc5510bb3ffd35fc1b0d986f34e33c51ab9/Src/Newtonsoft.Json/Converters/CustomCreationConverter.cs
                serializer.Populate(reader, dict);
            }

            return dict;
        }
    }
}