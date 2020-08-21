Consider this case:  
You have a piece of code that should be executed after some SubModule, e.g. [``StoryModeSubModule``](xref:StoryMode.StoryModeSubModule), but you don't want to add ``StoryMode`` as a ``DependedModule``.  
This is not supported out-of-the-box, but ``ButterLib`` provides one possible solution.  
```csharp
protected override void OnSubModuleLoad()
 {
    base.OnSubModuleLoad();

    DelayedSubModuleLoader.Register<StoryModeSubModule>();
    DelayedSubModuleLoader.Subscribe<StoryModeSubModule, SubModule>(nameof(OnSubModuleLoad),
        DelayedSubModuleSubscriptionType.AfterMethod, (s, e) =>
        {
            // StoryModeSubModule does not implement OnSubModuleLoad(), so we can only catch the base virtual method call.
            if (e.IsBase)
                return;

            // SOME CODE
        });
}
```
  
```csharp
protected override void OnBeforeInitialModuleScreenSetAsRoot()
 {
    base.OnBeforeInitialModuleScreenSetAsRoot();

    DelayedSubModuleLoader.Register<GauntletUISubModule>();
    DelayedSubModuleLoader.Subscribe<GauntletUISubModule, SubModule>(nameof(OnBeforeInitialModuleScreenSetAsRoot),
        DelayedSubModuleSubscriptionType.AfterMethod, (s, e) =>
        {
            // GauntletUISubModule overrides OnBeforeInitialModuleScreenSetAsRoot, so we can catch the override method call.
            if (!e.IsBase)
                return;

            // SOME CODE
        });
}
```

## Note:
* If for some reason your Module will load after the Module that you subscribed to, the delegate you passed in ``Subscribe`` will be executed immediately.
* If a derived [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase) class overrides the method you subcsribe to and calls the base.Method(), you will get two calls, one from the override and one from calling the empty virtual method. Don't forget to filter by [``DelayedSubModuleEventArgs.IsBase``](xref:Bannerlord.ButterLib.DelayedSubModule.DelayedSubModuleEventArgs.html#collapsible-Bannerlord_ButterLib_DelayedSubModule_DelayedSubModuleEventArgs_IsBase).
* The current implementation does not allow to subscribe to methods e.g. [``OnBeforeInitialModuleScreenSetAsRoot()``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) outside the [``OnBeforeInitialModuleScreenSetAsRoot()``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) override. You will not be able to subscribe to [``OnBeforeInitialModuleScreenSetAsRoot()``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) in [``OnSubModuleLoad()``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnSubModuleLoad) override and vise-versa.