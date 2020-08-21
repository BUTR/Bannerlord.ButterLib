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
            if (!e.IsValidBase<StoryModeSubModule>(nameof(OnSubModuleLoad), DelayedSubModuleSubscriptionType.AfterMethod))
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
            // GauntletUISubModule overrides OnBeforeInitialModuleScreenSetAsRoot, so we can catch the method call.
            if (!e.IsValid<GauntletUISubModule>(nameof(OnBeforeInitialModuleScreenSetAsRoot), DelayedSubModuleSubscriptionType.AfterMethod))
                return;

            // SOME CODE
        });
}
```

## Note:
* If for some reason your Module will load after the Module that you subscribed to, the delegate you passed in ``Subscribe`` will be executed immediately.
* The current implementation does not allow to subscribe to methods e.g. [``OnBeforeInitialModuleScreenSetAsRoot``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) outside the [``OnBeforeInitialModuleScreenSetAsRoot``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) override. You will not be able to subscribe to [``OnBeforeInitialModuleScreenSetAsRoot``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnBeforeInitialModuleScreenSetAsRoot) in [``OnSubModuleLoad``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase.html#collapsible-TaleWorlds_MountAndBlade_MBSubModuleBase_OnSubModuleLoad) override and vise-versa.