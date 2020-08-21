## [``MBSubModuleBaseWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseWrapper)
Wraps a [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) and gives the ability to easy call the ``protected internal`` methods like  [``OnSubModuleLoad()``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnSubModuleLoad).  
## [``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseListWrapper)
The same as previous, wraps multiple [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) instead of one.  
  
## Usage
[``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseListWrapper) could be used if you need to dynamically load specific SubModules based on some condition.
You need to create a [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) class that derives from [``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseListWrapper), in which you decide what assemblies to load.  
[``MBSubModuleBaseListWrapper``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseListWrapper) will check the [``SubModules``](xref:Bannerlord.ButterLib.SubModuleWrappers.MBSubModuleBaseListWrapper#collapsible-Bannerlord_ButterLib_SubModuleWrappers_MBSubModuleBaseListWrapper_SubModules) property for the SubModules and proxy any game call to them.  
Dont forget to include the new SubModule in ``SubModule.xml``!  
```xml
<SubModule>
  <Name value="Dynamic SubModule Loader" />
  <DLLName value="Module.dll" />
  <SubModuleClassType value="Module.DynamicSubModuleLoader" />
  <Tags/>
</SubModule>
```
See [``ImplementationLoaderSubModule``](xref:Bannerlord.ButterLib.ImplementationLoaderSubModule) for an example.  
