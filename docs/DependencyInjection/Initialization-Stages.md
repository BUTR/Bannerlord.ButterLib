Initialization Stages:  

* Stage 1 - ``PreInitialization`` - Before ``ButterLib`` is processed by the game. Developers can inject into ``ButterLib`` via ``ButterLibSubModule.ConfigureBeforeStart()``.  
Starts after the game starts loading SubModules.  
Ends when the game calls ``ButterLibSubModule.OnSubModuleLoad()``.  

* Stage 2 - ``Initialization`` - any SubModule can register their own services.  
Starts after the game calls ``ButterLibSubModule.OnSubModuleLoad()``.  
Ends when the game calls ``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``.  

* Stage 3 - ``PostInitialization`` - Services can be discovered by any SubModule.  
Starts after the game calls ``ButterLibSubModule.OnBeforeInitialModuleScreenSetAsRoot()``.  
