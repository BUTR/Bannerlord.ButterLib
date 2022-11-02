using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Options
{
    [JsonObject(MemberSerialization.OptIn)]
    internal sealed class JsonButterLibOptionsModel
    {
        //private static readonly string Path = System.IO.Path.Combine(Utilities.GetConfigsPath(), "ModSettings/Global/ButterLib/ButterLib_v1.json");
        private static readonly PlatformDirectoryPath BasePath = EngineFilePaths.ConfigsPath + "/ModSettings/ButterLib";

        [JsonProperty("MinLogLevel", DefaultValueHandling = DefaultValueHandling.Populate)]
        public int MinLogLevel { get; private set; } = (int) LogLevel.Information;

        public JsonButterLibOptionsModel()
        {
            var filePath = new PlatformFilePath(BasePath, "Options.json");

            if (FileHelper.FileExists(filePath))
            {
                try
                {
                    JsonConvert.PopulateObject(FileHelper.GetFileContentString(filePath), this);
                }
                catch (Exception e) when (e is JsonSerializationException)
                {
                    FileHelper.SaveFileString(filePath, JsonConvert.SerializeObject(this));
                }
                catch
                {
                    return;
                }
            }
            else
            {
                FileHelper.SaveFileString(filePath, JsonConvert.SerializeObject(this));
            }
        }

        private static void TryCreate(PlatformFilePath filePath, JsonButterLibOptionsModel model)
        {
            try
            {
            }
            catch { }
        }

        private static void TryOverwrite(PlatformFilePath filePath, JsonButterLibOptionsModel model)
        {
            try
            {
                FileHelper.SaveFileString(filePath, JsonConvert.SerializeObject(model));
            }
            catch { }
        }
    }
}