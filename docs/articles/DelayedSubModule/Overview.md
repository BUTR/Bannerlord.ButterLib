Consider this case:  
You have a piece of code that should be executed after some SubModule, e.g. [``StoryModeSubModule``](https://apidoc.bannerlord.com/v/1.2.7/class_story_mode_1_1_story_mode_sub_module.html), but you don't want to add ``StoryMode`` as a ``DependedModule``.  
This is not supported out-of-the-box, but ``ButterLib`` provides one possible solution.  
```csharp
protected override void OnSubModuleLoad()
 {
    base.OnSubModuleLoad();

    // You first call Register() so the SubModule will be tracked by ButterLib
    DelayedSubModuleManager.Register<StoryModeSubModule>();
    // You subscribe to the module's method call.
    // In this case, we do something after StoryModeSubModule.OnSubModuleLoad
    // is executed.
    DelayedSubModuleManager.Subscribe<StoryModeSubModule, SubModule>(
        nameof(OnSubModuleLoad), SubscriptionType.AfterMethod, (s, e) =>
        {
            // StoryModeSubModule does not implement OnSubModuleLoad(),
            // so we can only catch the base virtual method call.
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

    DelayedSubModuleManager.Register<GauntletUISubModule>();
    DelayedSubModuleManager.Subscribe<GauntletUISubModule, SubModule>(
        nameof(OnBeforeInitialModuleScreenSetAsRoot), SubscriptionType.AfterMethod, (s, e) =>
        {
            // GauntletUISubModule overrides OnBeforeInitialModuleScreenSetAsRoot, so we can
            // catch the override method call.
            if (!e.IsBase)
                return;

            // SOME CODE
        });
}
```

## Note:
* If for some reason your Module will load after the Module that you subscribed to, the delegate you passed in ``Subscribe`` will be executed immediately.
* If a derived [``MBSubModuleBase``](https://apidoc.bannerlord.com/v/1.2.7/class_tale_worlds_1_1_mount_and_blade_1_1_m_b_sub_module_base.html) class overrides the method you subcsribe to and calls the base.Method(), you will get two calls, one from the override and one from calling the empty virtual method. Don't forget to filter by [``SubscriptionEventArgs.IsBase``](xref:Bannerlord.ButterLib.DelayedSubModule.SubscriptionEventArgs#collapsible-Bannerlord_ButterLib_DelayedSubModule_SubscriptionEventArgs_IsBase).
* The current implementation does not allow to subscribe to methods e.g. ``OnBeforeInitialModuleScreenSetAsRoot()`` outside the ``OnBeforeInitialModuleScreenSetAsRoot()`` override. You will not be able to subscribe to ``OnBeforeInitialModuleScreenSetAsRoot()`` in ``OnSubModuleLoad()`` override and vise-versa.
