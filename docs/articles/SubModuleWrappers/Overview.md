## [``MBSubModuleBaseWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseWrapper)
Wraps a [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) and gives the ability to easy call the ``protected internal`` methods like  ``OnSubModuleLoad()``.  
## [``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseListWrapper)
The same as previous, wraps multiple [``MBSubModuleBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_mount_and_blade_1_1_m_b_sub_module_base.html) instead of one.  
  
## Usage
[``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseListWrapper) could be used if you need to dynamically load specific SubModules based on some condition.
You need to create a [``MBSubModuleBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_mount_and_blade_1_1_m_b_sub_module_base.html) class that derives from [``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseListWrapper), in which you decide what assemblies to load.  
[``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseListWrapper) will check the [``SubModules``](xref:Bannerlord.ButterLib.SubModuleWrappers2.MBSubModuleBaseListWrapper#collapsible-Bannerlord_ButterLib_SubModuleWrappers_MBSubModuleBaseListWrapper_SubModules) property for the SubModules and proxy any game call to them.  
Dont forget to include the new SubModule in ``SubModule.xml``!  
```xml
<SubModule>
  <Name value="Dynamic SubModule Loader" />
  <DLLName value="Module.dll" />
  <SubModuleClassType value="Module.DynamicSubModuleLoader" />
  <Tags/>
</SubModule>
```
```csharp
    public sealed class DynamicSubModuleLoader : MBSubModuleBaseListWrapper
    {
        protected override void OnSubModuleLoad()
        {
            // Add any MBSubModuleBase instance that should be handled.
            SubModules.AddRange(...);
            base.OnSubModuleLoad();
        }
    }
```

See [``ImplementationLoaderSubModule``](xref:Bannerlord.ButterLib.ImplementationLoaderSubModule) for an example.  
