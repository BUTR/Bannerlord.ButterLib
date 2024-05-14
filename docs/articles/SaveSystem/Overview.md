[``SaveSystem``](xref:Bannerlord.ButterLib.SaveSystem) provides wrappers and extensions to the game's save system.

# SyncDataAsJson

## The Old Way

To sync your custom data to savegames, you need to define a [``CampaignBehaviorBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_campaign_system_1_1_campaign_behavior_base.html) and override its [``SyncData(IDataStore dataStore)``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_campaign_system_1_1_campaign_behavior_base.html#a05ec45ba9a8707a6048fa5ba129fb438) method. From there, you'd make one or more calls to [``dataStore.SyncData<T>(string key, ref T myObject)``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802) to actually synchronize the data to the savegame.

This seems dandy right up until you realize that it's actually quite a pain in practice, because in many cases, [``SyncData<T>``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802) will not only fail to work correctly with some standard types (e.g., simple containers like [``Dictionary<string, string>``](xref:xref:System.Collections.Generic.Dictionary) but also the game's implementation will often force the save files to depend upon your mod being loaded &mdash; not to mention the need to define your own ``SaveableTypeDefiner`` and pick arbitrary unique "base IDs" for your mod just to get anything done. If the save file ends up depending upon your mod being loaded, then your users are unnecessarily screwed when they disable it.

In short, the old way is a pretty decent try, but it falls short on safely removing your mod from a savegame in all cases, the general amount of error-prone configuration required, too many of your wasted hours trying to synchronize data types that should obviously be handled by default but simply aren't, and wasted time marshalling things like ``MBGUID``s back to object references.

## The New Way: SyncDataAsJson

We've developed a drop-in replacement for the aforementioned [``SyncData<T>``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802): [``SyncDataAsJson<T>``](xref:Bannerlord.ButterLib.SaveSystem.Extensions.IDataStoreExtensions#collapsible-Bannerlord_ButterLib_SaveSystem_Extensions_IDataStoreExtensions_SyncDataAsJson__1_TaleWorlds_CampaignSystem_IDataStore_System_String___0__Newtonsoft_Json_JsonSerializerSettings_). It doesn't suffer from any of the aforementioned issues, and as an extension method of [``IDataStore``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html), you use it exactly like you would've used [``SyncData<T>``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802). However, your custom data is now, behind the scenes, serialized by ButterLib into a simple ``string`` (or similar primitive type).

[``SyncDataAsJson<T>``](xref:Bannerlord.ButterLib.SaveSystem.Extensions.IDataStoreExtensions#collapsible-Bannerlord_ButterLib_SaveSystem_Extensions_IDataStoreExtensions_SyncDataAsJson__1_TaleWorlds_CampaignSystem_IDataStore_System_String___0__Newtonsoft_Json_JsonSerializerSettings_) doing its own serialization to a primitive type behind the scenes means:

* Your mod will never save custom data that prevents the game from loading properly when your mod is disabled

* You will never need to define another ``SaveableTypeDefiner``

* Far more standard types, especially involving standard containers, will be handled automatically, and in the off chance that they aren't, you (and we) have the power to add custom type serializers.

* You will never again need to manually save ``MBGUID`` values or manually restore them to [``MBObjectBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_base.html)-derived object references in order to properly save, say, a [``Clan``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_campaign_system_1_1_clan.html) reference again.

The serialization engine, [``Newtonsoft.Json``](https://github.com/JamesNK/Newtonsoft.Json), in order to allow [``SyncDataAsJson<T>``](xref:Bannerlord.ButterLib.SaveSystem.Extensions.IDataStoreExtensions#collapsible-Bannerlord_ButterLib_SaveSystem_Extensions_IDataStoreExtensions_SyncDataAsJson__1_TaleWorlds_CampaignSystem_IDataStore_System_String___0__Newtonsoft_Json_JsonSerializerSettings_) to operate as a drop-in replacement for the old method, has been outfit with a [custom contract resolver](xref:Bannerlord.ButterLib.SaveSystem.TaleWorldsContractResolver) and a number of special type converters.

The engine's custom contract resolver will only serialize data tagged with the TaleWorlds ``SaveableField`` or ``SaveableProperty`` attributes. Likewise, custom types intended for serialization must still use the ``SaveableClass`` or ``SaveableStruct`` attributes. Note that the ID numbers required by these attributes are an artifact of the old system and don't actually matter to [``SyncDataAsJson<T>``](xref:Bannerlord.ButterLib.SaveSystem.Extensions.IDataStoreExtensions#collapsible-Bannerlord_ButterLib_SaveSystem_Extensions_IDataStoreExtensions_SyncDataAsJson__1_TaleWorlds_CampaignSystem_IDataStore_System_String___0__Newtonsoft_Json_JsonSerializerSettings_), but you're advised to fill them out like normal for new types in the event that you need to go back to [``SyncData<T>``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802) for some reason.

An example usage follows. Remember, the only thing that's really changed here is using [``SyncDataAsJson<T>``](xref:Bannerlord.ButterLib.SaveSystem.Extensions.IDataStoreExtensions#collapsible-Bannerlord_ButterLib_SaveSystem_Extensions_IDataStoreExtensions_SyncDataAsJson__1_TaleWorlds_CampaignSystem_IDataStore_System_String___0__Newtonsoft_Json_JsonSerializerSettings_) instead of [``SyncData<T>``](https://apidoc.bannerlord.com/v/1.2.7/interface_tale_worlds_1_1_campaign_system_1_1_i_data_store.html#a8f476030a56cf3423bd9e2912c4e5802).

```csharp
using Bannerlord.ButterLib.SaveSystem.Extensions;

public class CustomBehavior : CampaignBehaviorBase
{
    public override void SyncData(IDataStore dataStore)
    {
        dataStore.SyncDataAsJson("KeyForMyClass", ref _myClassObj);
        // ... perhaps more SyncDataAsJson calls for other data ...
    }

    [Serializable] // If the struct/class has the Serializable attribute`SaveableField and SaveableProperty will be ignored
    public MyStruct
    {
        public int X;
        public int Y;
        public int Z;
    }

    private MyClass // Why private? Just to point out that access levels aren't an issue.
    {
        public int _unsavedField = 42;

        [SaveableField(1)]
        public Dictionary<Hero, int> _heroButterGiftAmounts = new Dictionary<Hero, int>();

        [SaveableProperty(2)]
        public string Name { get; set; } = "The Butter Lord";
    }

    private MyClass _myClassObj = new MyClass();

    // ... other campaign behavior code to, presumably, give a lot of butter away everyday
}
```

This extension is the next step in the evolution of best practices and just plain less frustrating practices for the synchronization of your mod's custom data to savegames.

### Notes:

* Built-in [``MBObjectBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_base.html)-derived types (e.g., [``Hero``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_campaign_system_1_1_hero.html) or [``Town``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_campaign_system_1_1_settlements_1_1_town.html)) have a [custom converter](xref:Bannerlord.ButterLib.SaveSystem.MBObjectBaseConverter). They are serialized as their [``Id``](xref:TaleWorlds.ObjectSystem.MBObjectBase#collapsible-TaleWorlds_ObjectSystem_MBObjectBase_Id) property. [``MBObjectManager``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_manager.html) is used to resolve these numeric IDs to the correct, live game object references at deserialization time.

* Custom [``MBObjectBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_base.html) types are not serialized (i.e., custom types derived from [``MBObjectBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_base.html) that aren't registered with the game's official object manager). While we do not know if such types even exist, we consider this to be non-ideal and intend to fix it in the future for completeness. One of the proposed solutions is to have our own registry of such custom objects and to resolve them from it &mdash; basically a custom [``MBObjectManager``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_object_system_1_1_m_b_object_manager.html).
