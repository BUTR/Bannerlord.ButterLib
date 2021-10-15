# MBSubModuleBase extended
Introduces an interface to use in your [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase)-derived class, that provides new SubModule events for additional control over crucial parts of game and campaign loading or unloading.

## Usage
To access additional events you should derive your submodule from [``MBSubModuleBaseEx``](xref:Bannerlord.ButterLib.MBSubModuleBaseExtended.MBSubModuleBaseEx) instead of [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase).
```csharp
public class SubModule : MBSubModuleBaseEx
{
    public override void OnAllSubModulesUnLoaded()
    {
        base.OnAllSubModulesUnLoaded();
    }

    public override void OnBeforeInitialModuleScreenSetAsRootDelayed()
    {
        base.OnBeforeInitialModuleScreenSetAsRootDelayed();
    }

    public override void OnGameStartDelayed(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStartDelayed(game, gameStarterObject);
    }

    public override void OnGameEndDelayed(Game game)
    {
        base.OnGameEndDelayed(game);
    }
}
```
Alternatively, you can derive directly from the [``IMBSubModuleBaseEx``](xref:Bannerlord.ButterLib.MBSubModuleBaseExtended.IMBSubModuleBaseEx) interface. This way you can derive from other virual or abstract classes and still access all the additional events, introduced by the MBSubModuleBase extended.
```csharp
public class SubModule : MBSubModuleBaseWrapper, IMBSubModuleBaseEx
{
        public void OnAllSubModulesUnLoaded()
        {
            //Leave empty or add an implementation here
        }

        public void OnBeforeInitialModuleScreenSetAsRootDelayed()
        {
            //Leave empty or add an implementation here
        }

        public void OnGameStartDelayed(Game game, IGameStarter gameStarterObject)
        {
            //Leave empty or add an implementation here
        }

        public void OnGameEndDelayed(Game game)
        {
            //Leave empty or add an implementation here
        }
}
```
