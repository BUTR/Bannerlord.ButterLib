Initialization Stages:  

* Stage 1 - ``PreInitialization`` - Before ``ButterLib`` is processed by the game. Developers can inject into ``ButterLib`` via [``ButterLibSubModule.ConfigureBeforeInitialization()``](xref:Bannerlord.ButterLib.ButterLibSubModule#collapsible-Bannerlord_ButterLib_ButterLibSubModule_ConfigureBeforeInitialization_System_Action_Microsoft_Extensions_DependencyInjection_IServiceCollection__).  
Stage starts after the game starts loading SubModules.  
Stage ends when the game calls [``ButterLibSubModule.OnSubModuleLoad()``](xref:Bannerlord.ButterLib.ButterLibSubModule#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnSubModuleLoad).  

* Stage 2 - ``Initialization`` - any [``MBSubModuleBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_mount_and_blade_1_1_m_b_sub_module_base.html) can register their own services.  
Stage starts after the game calls [``ButterLibSubModule.OnSubModuleLoad()``](xref:Bannerlord.ButterLib.ButterLibSubModule#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnSubModuleLoad).  
Stage ends when the game calls [``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``](xref:Bannerlord.ButterLib.ButterLibSubModule#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnBeforeInitialModuleScreenSetAsRoot).  

* Stage 3 - ``PostInitialization`` - Services can be discovered by any [``MBSubModuleBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_mount_and_blade_1_1_m_b_sub_module_base.html).  
Stage starts after the game calls [``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``](xref:Bannerlord.ButterLib.ButterLibSubModule#collapsible-Bannerlord_ButterLib_ButterLibSubModule_OnBeforeInitialModuleScreenSetAsRoot).  
