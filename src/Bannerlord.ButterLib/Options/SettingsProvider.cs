using Bannerlord.ButterLib.SubSystems;
using Bannerlord.ButterLib.SubSystems.Settings;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Linq.Expressions;
using System.Reflection;

using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace Bannerlord.ButterLib.Options;

internal class SettingsProvider
{
    private static PropertyInfo? GetPropertyInfo<T, TResult>(Expression<Func<T, TResult>> expression) =>
        expression is LambdaExpression { Body: MemberExpression { Member: PropertyInfo propertyInfo } } ? propertyInfo : null;

    private static bool Read<TSubSystem>(TSubSystem subSystem, JsonReader reader) where TSubSystem : ISubSystem
    {
        var jo = JObject.Load(reader);

        var isEnabled = true;
        var enabledId = $"{typeof(TSubSystem).Name} Enabled";
        if (jo.TryGetValue(enabledId, out var isEnabledValue))
            isEnabled = isEnabledValue.ToObject<bool>();

        if (subSystem is not ISubSystemSettings<TSubSystem> settings)
            return isEnabled;


        foreach (var declaration in settings.Declarations)
        {
            var func = declaration switch
            {
                SubSystemSettingsPropertyBool<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi => () =>
                {
                    var id = $"{typeof(TSubSystem).Name}_{pi.Name}";
                    if (jo.TryGetValue(id, out var value))
                        pi.SetValue(subSystem, value.ToObject(pi.PropertyType));
                }
                ,
                SubSystemSettingsPropertyDropdown<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi => () =>
                {
                    var id = $"{typeof(TSubSystem).Name}_{pi.Name}";
                    if (jo.TryGetValue(id, out var value))
                        sp.SelectedIndex = value.ToObject<int>();
                }
                ,
                SubSystemSettingsPropertyFloat<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi => () =>
                {
                    var id = $"{typeof(TSubSystem).Name}_{pi.Name}";
                    if (jo.TryGetValue(id, out var value))
                        pi.SetValue(subSystem, value.ToObject(pi.PropertyType));
                }
                ,
                SubSystemSettingsPropertyInt<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi => () =>
                {
                    var id = $"{typeof(TSubSystem).Name}_{pi.Name}";
                    if (jo.TryGetValue(id, out var value))
                        pi.SetValue(subSystem, value.ToObject(pi.PropertyType));
                }
                ,
                SubSystemSettingsPropertyText<TSubSystem> sp when GetPropertyInfo(sp.Property) is { } pi => () =>
                {
                    var id = $"{typeof(TSubSystem).Name}_{pi.Name}";
                    if (jo.TryGetValue(id, out var value))
                        pi.SetValue(subSystem, value.ToObject(pi.PropertyType));
                }
                ,
                _ => (Action?) null,
            };
            func?.Invoke();
        }

        return isEnabled;
    }


    private static readonly PlatformDirectoryPath BasePath = EngineFilePaths.ConfigsPath + "/ModSettings/ButterLib";
    private static readonly PlatformFilePath optionsFilePath = new(BasePath, "Options.json");
    public static bool? PopulateSubSystemSettings<TSubSystem>(TSubSystem subSystem) where TSubSystem : ISubSystem
    {
        if (!FileHelper.FileExists(optionsFilePath))
            return null;

        var json = FileHelper.GetFileContentString(optionsFilePath);

        var reader = new JsonTextReader(new System.IO.StringReader(json));

        return Read(subSystem, reader);
    }

    public static ButterLibOptions GetSettings()
    {
        if (FileHelper.FileExists(optionsFilePath))
        {
            try
            {
                return JsonConvert.DeserializeObject<ButterLibOptions>(FileHelper.GetFileContentString(optionsFilePath)) ?? new();
            }
            catch (Exception e) when (e is JsonSerializationException)
            {
                FileHelper.SaveFileString(optionsFilePath, JsonConvert.SerializeObject(new()));
            }
            catch { /* ignore */ }
        }
        else
        {
            FileHelper.SaveFileString(optionsFilePath, JsonConvert.SerializeObject(new()));
        }

        return new();
    }
}