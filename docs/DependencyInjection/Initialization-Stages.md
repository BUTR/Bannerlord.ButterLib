Initialization Stages:  

* Stage 1 - ``PreInitialization`` - Before ``ButterLib`` is processed by the game. Developers can inject into ``ButterLib`` via ``ButterLibSubModule.ConfigureBeforeInitialization()``.  
Stage starts after the game starts loading SubModules.  
Stage ends when the game calls ``ButterLibSubModule.OnSubModuleLoad()``.  

* Stage 2 - ``Initialization`` - any ``SubModule`` can register their own services.  
Stage starts after the game calls ``ButterLibSubModule.OnSubModuleLoad()``.  
Stage ends when the game calls ``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``.  

* Stage 3 - ``PostInitialization`` - Services can be discovered by any ``SubModule``.  
Stage starts after the game calls ``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``.  
