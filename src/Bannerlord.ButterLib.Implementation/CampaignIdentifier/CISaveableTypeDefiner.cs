using Bannerlord.ButterLib.Implementation.CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

using System.Collections.Generic;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    internal sealed class CISaveableTypeDefiner : ButterLibSaveableTypeDefiner
    {
        internal CISaveableTypeDefiner() : base(0) { }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(CampaignDescriptorImplementation), 1);
            AddClassDefinition(typeof(CampaignDescriptorManager), 2);
        }

        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(CampaignDescriptorImplementation.DescriptorAttribute), 3);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<CampaignDescriptorImplementation.DescriptorAttribute, object>));
        }
    }
}