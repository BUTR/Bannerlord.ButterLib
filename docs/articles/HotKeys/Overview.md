# HotKeys

The `HotKeyManager` type and associated views and functionality inside of Bannerlord is incredibly convoluted to work with, there are lots of gotchas and having conflicts between different mods registering hotkeys with the same ID's or mod names is almost a certainty, so this simple wrapper around it was created that handles the finer details and handles input polling so the entire thing is event based.

## Usage

```csharp
public class MySubModule : MBSubModuleBase
{
    private bool _campaignIsStarted;
    protected override void OnBeforeInitialModuleScreenSetAsRoot()
    {
        // Create a new HotKeyManager for your mod.
        var hkm = HotKeyManager.Create("MyMod");
        // Add your HotKeyBase derived class to the manager.
        // You can add as many hotkeys as you'd like before building them up.
        // You can also use `hkm.Add(new TestKey(SomeExampleArgument))` if you'd like to have a non-default constructor.
        var rslt = hkm.Add<TestKey1>();
        // var rslt = hkm.Add<TestKey2>();
        // It's not necessary to supply a predicate, it's just a convenience.
        // You can also manually set IsEnabled to more simply enable/disable a keys functionality.
        rslt.Predicate = () => _campaignIsStarted;
        // Subscribe to each of the events on the hotkey at any time.
        rslt.OnReleasedEvent += () =>
            InformationManager.DisplayMessage(new InformationMessage("Test Key Released!", Colors.Magenta));
        // Call this to build up all the hotkeys you added.
        hkm.Build();
    }

    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        // An example just to demonstrate functionality.
        if (game.GameType is Campaign) _campaignIsStarted = true;
    }
}

public class TestKey1 : HotKeyBase
{
    protected override string DisplayName { get; }
    protected override string Description { get; }
    protected override InputKey DefaultKey { get; }
    protected override string Category { get; }

    public TestKey1() : base(nameof(TestKey1))
    {
        DisplayName = "My Test Key";
        Description = "This is a test key.";
        DefaultKey = InputKey.Comma;
        Category = HotKeyManager.Categories[HotKeyCategory.CampaignMap];
    }

    protected override void OnReleased()
    {
        // You can also override methods relating to keypresses within the key itself.
    }
}
public class TestKey2 : HotKeyBase
{
    public TestKey2() : base(nameof(TestKey2), "My Test Key", "This is a test key.", InputKey.Comma, HotKeyManager.Categories[HotKeyCategory.CampaignMap]) { }

    protected override void OnReleased()
    {
        // You can also override methods relating to keypresses within the key itself.
    }
}
```

# Credit
Original code taken from [BannerLib](https://github.com/sirdoombox/BannerLib/tree/d721fb572f33a702ab7b724b866fe06d86d60d1a/BannerLib.Input) by [sirdoombox](https://github.com/sirdoombox).