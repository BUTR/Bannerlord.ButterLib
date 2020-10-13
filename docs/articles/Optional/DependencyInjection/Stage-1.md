This is the stage before ``ButterLib`` gets the opportunity to do the first initialization steps.  
It is useful for overriding ``ButterLib``'s default behavior.  
[``MBSubModuleBase.GetServices()``](xref:Bannerlord.ButterLib.Common.Extensions.DependencyInjectionExtensions.html%23collapsible-Bannerlord_ButterLib_Common_Extensions_DependencyInjectionExtensions_GetServices_TaleWorlds_MountAndBlade_MBSubModuleBase_) is not available at this stage.  
  
Right now ``ButterLib`` is loading it's options from a fixed file. This can be overriden, so the config file would be loaded from another place, e.g from ``MCM``'s ``ModOptions`` folder.  
