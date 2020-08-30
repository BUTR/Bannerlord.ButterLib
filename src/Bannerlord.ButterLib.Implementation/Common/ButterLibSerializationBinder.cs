using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bannerlord.ButterLib.Implementation.Common
{
    internal sealed class ButterLibSerializationBinder : SerializationBinder
    {
        public override Type? BindToType(string assemblyName, string typeName)
        {
            if (assemblyName.StartsWith("Bannerlord.ButterLib.Implementation"))
                return typeof(ButterLibSerializationBinder).Assembly.GetType(typeName);

            var type = Type.GetType($"{typeName}, {assemblyName}");
            if (type != null)
                return type;

            var tokens = typeName.Split(new []  {"[[", "]]", "],["}, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 1)
                return Type.GetType(typeName, true);

            var generic = tokens[0];
            var genericTypeArgs = new List<string>();
            foreach (var token in tokens.Skip(1))
            {
                var (typeName1, assemblyName1) = GetTokenInfo(token);
                var type1 = assemblyName1.StartsWith("Bannerlord.ButterLib.Implementation")
                    ? typeof(ButterLibSerializationBinder).Assembly.GetType(typeName1)
                    : Type.GetType($"{typeName1}, {assemblyName1}", true);
                genericTypeArgs.Add(type1.AssemblyQualifiedName);
            }

            return Type.GetType($"{generic}[[{string.Join("],[", genericTypeArgs)}]]", true);
        }

        private static (string TypeName, string AssemblyName) GetTokenInfo(string str)
        {
            var split = str.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return (split[0].Trim(), string.Join(",", split.Skip(1)).Trim());
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = "Bannerlord.ButterLib.Implementation";
            typeName     = serializedType.FullName!;
        }
    }
}