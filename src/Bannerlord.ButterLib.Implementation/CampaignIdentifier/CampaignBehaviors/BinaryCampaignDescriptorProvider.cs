#if e143 || e150 || e151 || e152 || e153
using Bannerlord.ButterLib.CampaignIdentifier;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors
{
    internal sealed class BinaryCampaignDescriptorProvider : ICampaignDescriptorProvider
    {
        private static readonly string ExistingCampaignDescriptorsLogFile =
            Path.Combine(Utilities.GetConfigsPath(), "ButterLib", "CampaignIdentifier", "ExistingCampaignIdentifiers.bin");

        public IEnumerable<CampaignDescriptor> Load()
        {
            try
            {
                var path = Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile);
                if (string.IsNullOrEmpty(path) || !Directory.Exists(path) || !File.Exists(ExistingCampaignDescriptorsLogFile))
                    return Enumerable.Empty<CampaignDescriptor>();

                using var fileStream = File.OpenRead(ExistingCampaignDescriptorsLogFile);
                var binaryFormatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple,
                    Binder = new ButterLibSerializationBinder()
                };
                return binaryFormatter.Deserialize(fileStream) is List<CampaignDescriptorImplementation> list
                    ? list
                    : Enumerable.Empty<CampaignDescriptor>();
            }
            catch (Exception e) when (e is SerializationException)
            {
                return Enumerable.Empty<CampaignDescriptor>();
            }
        }

        public void Save(IEnumerable<CampaignDescriptor> descriptors)
        {
            var path = Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile);
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path!);

            using var fileStream = File.OpenWrite(ExistingCampaignDescriptorsLogFile);
            var binaryFormatter = new BinaryFormatter
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple,
                Binder = new ButterLibSerializationBinder()
            };
            binaryFormatter.Serialize(fileStream, descriptors.ToList());
        }

        internal sealed class ButterLibSerializationBinder : SerializationBinder
        {
            public override Type? BindToType(string assemblyName, string typeName)
            {
                if (assemblyName.StartsWith("Bannerlord.ButterLib.Implementation"))
                    return typeof(ButterLibSerializationBinder).Assembly.GetType(typeName);

                var type = Type.GetType($"{typeName}, {assemblyName}");
                if (type is not null)
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
                    if (type1 is not null && type1.AssemblyQualifiedName is not null)
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
}
#endif