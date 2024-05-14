# Distance Matrix
Distance Matrix is a subsystem that provides a means for handling distances between various `MBObjectBase` objects in the game. You can use it to create your own implementations for the types you need.
Additionally, there are built-in implementations for `Settlement`, `Clan`, and `Kingdom` types, along with a behavior to keep the distances updated.

## Usage
Use [``DistanceMatrix``](xref:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix) class to work with your custom distance matrix.
Use [``CampaignExtensions``](xref:Bannerlord.ButterLib.Common.Extensions.CampaignExtensions) to access the built-in implementations.

If you plan to use built-in implementations and behavior, don't forget to enable the SubSystem in your `SubModule` class:
```csharp
if (this.GetServiceProvider() is { } serviceProvider)
{
    var distanceMatrixSubSystem = serviceProvider.GetSubSystem("Distance Matrix");
    distanceMatrixSubSystem?.Enable();
}
```

Example usage of built-in `DistanceMatrix` for `Clan` type:
```csharp
var clanDistanceMatrix = Campaign.Current.GetDefaultClanDistanceMatrix();

var playerClan = Clan.PlayerClan;
var playerNeighbors = clanDistanceMatrix.GetNearestNeighbors(playerClan, 10);

Clan inquiredClan = Clan.All.FirstOrDefault(clan => clan.Fiefs.Count > 0 && Clan.All.Any(x => x.Fiefs.Count > 0 && clan.MapFaction.IsAtWarWith(x.MapFaction)));
var unfriendlyNeighbors = clanDistanceMatrix.GetNearestNeighbors(inquiredObject: inquiredClan, 20, x => !float.IsNaN(x.Distance) && x.OtherObject != inquiredClan && x.OtherObject.MapFaction.IsAtWarWith(inquiredClan.MapFaction)).ToList();
var unfriendlyNeighborsN = clanDistanceMatrix.GetNearestNeighborsNormalized(inquiredObject: inquiredClan, 20, x => !float.IsNaN(x.Distance) && x.OtherObject != inquiredClan && x.OtherObject.MapFaction.IsAtWarWith(inquiredClan.MapFaction)).ToList();
```

Example usage of Distance Matrix with custom selector and distance calculator:
```csharp
//Gives same result as Campaign.Current.GetDefaultClanDistanceMatrix();
//...or Campaign.Current.GetCampaignBehavior<GeopoliticsBehavior>().ClanDistanceMatrix;
var settlementDistanceMatrix = Campaign.Current.GetCampaignBehavior<GeopoliticsBehavior>().SettlementDistanceMatrix ?? new DistanceMatrixImplementation<Settlement>();
var clanDistanceMatrix = DistanceMatrix<Clan>.Create(() => Clan.All.Where(x => !x.IsEliminated && !x.IsBanditFaction), (clan, otherClan, args) =>
{
    if (args != null && args.Length == 1 && args[0] is Dictionary<ulong, WeightedDistance> lst)
    {
        return ButterLib.DistanceMatrix.DistanceMatrix.CalculateDistanceBetweenClans(clan, otherClan, lst).GetValueOrDefault();
    }
    return float.NaN;
}, [ButterLib.DistanceMatrix.DistanceMatrix.GetSettlementOwnersPairedList(settlementDistanceMatrix!)!]);
```
