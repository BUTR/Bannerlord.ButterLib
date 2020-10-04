using Bannerlord.ButterLib.CampaignIdentifier;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TaleWorlds.Engine;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors
{
    internal sealed class JsonCampaignDescriptorProvider : ICampaignDescriptorProvider
    {
        private static readonly string ExistingCampaignDescriptorsLogFile =
            Path.Combine(Utilities.GetConfigsPath(), "ButterLib", "CampaignIdentifier", "Existing.json");

        public IEnumerable<CampaignDescriptor> Load()
        {
            var path = Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile);
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path) || !File.Exists(ExistingCampaignDescriptorsLogFile))
                return Enumerable.Empty<CampaignDescriptor>();

            var success = true;
            var settings = new JsonSerializerSettings
            {
                Error = (sender, args) => { success = false; args.ErrorContext.Handled = true; },
                MissingMemberHandling = MissingMemberHandling.Error
            };
            using var fileStream = File.OpenRead(ExistingCampaignDescriptorsLogFile);
            var buffer = ReadFully(fileStream);
            var result = JsonConvert.DeserializeObject<IEnumerable<CampaignDescriptor>>(Encoding.UTF8.GetString(buffer), settings);
            return success ? result : Enumerable.Empty<CampaignDescriptor>();
        }

        public void Save(IEnumerable<CampaignDescriptor> descriptors)
        {
            var path = Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile);
            if (!string.IsNullOrEmpty(path) && !Directory.Exists(path))
                Directory.CreateDirectory(path!);

            using var fileStream = File.OpenWrite(ExistingCampaignDescriptorsLogFile);
            using var writer = new StreamWriter(fileStream);
            var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(descriptors, Formatting.Indented));
            fileStream.Write(buffer, 0, buffer.Length);
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16*1024];
            using MemoryStream ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, read);
            return ms.ToArray();
        }
    }
}