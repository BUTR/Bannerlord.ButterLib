# MBSubModuleBase extended
Introduces a [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase)-derived abstract class, that provides new SubModule events for additional control over crucial parts of game and campaign loading or unloading.

## Usage
To access additional events you should derive your submodule from [``MBSubModuleBaseEx``](xref:Bannerlord.ButterLib.MBSubModuleBaseExtended.MBSubModuleBaseEx) instead of [``MBSubModuleBase``](xref:TaleWorlds.MountAndBlade.MBSubModuleBase).
```csharp
public class SubModule : MBSubModuleBaseEx
{
    protected internal override void OnAllSubModulesUnLoaded()
    {
        base.OnAllSubModulesUnLoaded();
    }

    protected internal override void OnBeforeInitialModuleScreenSetAsRootDelayed()
    {
        base.OnBeforeInitialModuleScreenSetAsRootDelayed();
    }

    protected internal override void OnGameStartDelayed(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStartDelayed(game, gameStarterObject);
    }

    protected internal override void OnGameEndDelayed(Game game)
    {
        base.OnGameEndDelayed(game);
    }
}
```
