using System.Collections.Generic;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    public interface ICampaignDescriptorProvider
    {
        IEnumerable<CampaignDescriptor> Load();
        void Save(IEnumerable<CampaignDescriptor> descriptors);
    }
}