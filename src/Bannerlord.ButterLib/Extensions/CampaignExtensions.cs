using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Extensions;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.CampaignSystem;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Extensions
{
    /// <summary>
    /// Helper extension of the <see cref="Campaign" /> class
    /// returning additional information, provided by the ButterLib.
    /// </summary>
    /// <remarks>
    /// Contains easy accessible getters for the current CampaignId and <see cref="CampaignDescriptor" />
    /// provided by CampaignIdentifier service as well as various geopolitical distance matrices
    /// held in <see cref="T:Bannerlord.ButterLib.DistanceMatrix.DistanceMatrix`1" /> objects.
    /// </remarks>
    public static partial class CampaignExtensions
    {
        private static ICampaignExtensions? _instance;
        private static ICampaignExtensions? Instance =>
            _instance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ICampaignExtensions>();
    }
}