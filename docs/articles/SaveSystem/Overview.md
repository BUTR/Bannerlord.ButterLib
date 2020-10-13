[``SaveSystem``](xref:Bannerlord.ButterLib.SaveSystem) provides helper methods for the game's save system.   

## SyncDataAsJson
To sync your save data you need to define a [``CampaignBehaviorBase``](xref:TaleWorlds.CampaignSystem.CampaignBehaviorBase) and override [``SyncData(IDataStore dataStore)``](xref:TaleWorlds.CampaignSystem.CampaignBehaviorBase#collapsible-TaleWorlds_CampaignSystem_CampaignBehaviorBase_SyncData_TaleWorlds_CampaignSystem_IDataStore_) for the game to pickup your custom data. The problem is, the game's implementation will make the save file depend on your mod, thus making loading it without your mod impossible.  
Instead of using the game's serialization logic, we serialize the class ourselves and store it as one of the pre-defined data primitives - [``string``](xref:System.String).  
[``Newtonsoft.Json``](https://github.com/JamesNK/Newtonsoft.Json) is used for serializing the class into the string and vise versa. We use a [custom contract resolver](xref:Bannerlord.ButterLib.SaveSystem.TaleWorldsContractResolver) to detect [``SaveableField``](xref:TaleWorlds.SaveSystem.SaveableFieldAttribute) and [``SaveableProperty``](xref:TaleWorlds.SaveSystem.SaveablePropertyAttribute) and serialize the actual data.  
```csharp
public class CustomBehavior : CampaignBehaviorBase
{
    public override void SyncData(IDataStore dataStore)
    {
        dataStore.SyncDataAsJson("_descriptorManager", ref _descriptorManager);
    }
    public override void RegisterEvents() { }
}
```
With this extension method you will be able to use the game's save system to store optional data that will not break the game once your mod is removed!
