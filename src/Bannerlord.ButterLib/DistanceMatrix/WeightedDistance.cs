namespace Bannerlord.ButterLib.DistanceMatrix;

/// <summary>
/// Weighted distance between two objects.
/// </summary>
/// <param name="Distance">Distance between a pair of objects</param>
/// <param name="Weight">Weight of objects</param>
public record WeightedDistance(float Distance, float Weight);