using Bannerlord.ButterLib.CampaignIdentifier;

using System.Collections.Generic;
using System.Linq;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors
{
    internal sealed class BlankCampaignDescriptorProvider : ICampaignDescriptorProvider
    {
        public IEnumerable<CampaignDescriptor> Load() => Enumerable.Empty<CampaignDescriptor>();
        public void Save(IEnumerable<CampaignDescriptor> descriptors) { }
    }
}