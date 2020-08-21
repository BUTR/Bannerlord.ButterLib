Initialization Stages:  

* Stage 1 - ``PreInitialization`` - Before ``ButterLib`` is processed by the game. Developers can inject into ``ButterLib`` via [``ButterLibSubModule.ConfigureBeforeInitialization()``](xref:Bannerlord.ButterLib.ButterLibSubModule.html#collapsible-Bannerlord_ButterLib_ButterLibSubModule_ConfigureBeforeInitialization_System_Action_Microsoft_Extensions_DependencyInjection_IServiceCollection__).  
Stage starts after the game starts loading SubModules.  
Stage ends when the game calls [``ButterLibSubModule.OnSubModuleLoad()``](xref:Bannerlord.ButterLib.ButterLibSubModule.html#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnSubModuleLoad).  

* Stage 2 - ``Initialization`` - any [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) can register their own services.  
Stage starts after the game calls [``ButterLibSubModule.OnSubModuleLoad()``](xref:Bannerlord.ButterLib.ButterLibSubModule.html#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnSubModuleLoad).  
Stage ends when the game calls [``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``](xref:Bannerlord.ButterLib.ButterLibSubModule.html#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnBeforeInitialModuleScreenSetAsRoot).  

* Stage 3 - ``PostInitialization`` - Services can be discovered by any [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase).  
Stage starts after the game calls [``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``](xref:Bannerlord.ButterLib.ButterLibSubModule.html#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnBeforeInitialModuleScreenSetAsRoot).  
