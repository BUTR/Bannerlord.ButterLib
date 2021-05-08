using Bannerlord.BUTR.Shared.Helpers;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.IO;

namespace Bannerlord.ButterLib.Options
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class JsonButterLibOptionsModel
    {
        //private static readonly string Path = System.IO.Path.Combine(Utilities.GetConfigsPath(), "ModSettings/Global/ButterLib/ButterLib_v1.json");
        private static readonly string Path = System.IO.Path.Combine(FSIOHelper.GetConfigPath(), "ModSettings/ButterLib/Options.json");

        [JsonProperty("MinLogLevel", DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinLogLevel { get; private set; } = (int) LogLevel.Information;

        public JsonButterLibOptionsModel()
        {
            var file = new FileInfo(Path);

            if (file.Directory is null)
                throw new NullReferenceException($"Directory for path {Path} is null!");

            if (!file.Directory.Exists)
                file.Directory.Create();

            if (file.Exists)
            {
                try
                {
                    using var fs = file.OpenRead();
                    using var sr = new StreamReader(fs);
                    JsonConvert.PopulateObject(sr.ReadToEnd(), this);
                }
                catch (Exception e) when (e is JsonSerializationException)
                {
                    using var fs = file.OpenWrite();
                    using var sw = new StreamWriter(fs);
                    sw.WriteLine(JsonConvert.SerializeObject(this));
                }
            }
            else
            {
                using var fs = file.Create();
                using var sw = new StreamWriter(fs);
                sw.WriteLine(JsonConvert.SerializeObject(this));
            }
        }
    }
}