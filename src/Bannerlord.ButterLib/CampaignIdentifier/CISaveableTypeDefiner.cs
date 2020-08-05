using Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

using System.Collections.Generic;

using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    internal class CISaveableTypeDefiner : SaveableTypeDefiner
    {
        public CISaveableTypeDefiner() : base(2011750000) { }

        protected override void DefineClassTypes()
        {
            AddClassDefinition(typeof(CampaignDescriptor), 1);
            AddClassDefinition(typeof(CampaignDescriptorManager), 2);
        }

        protected override void DefineEnumTypes()
        {
            AddEnumDefinition(typeof(CampaignDescriptor.DescriptorAttribute), 3);
        }

        protected override void DefineContainerDefinitions()
        {
            ConstructContainerDefinition(typeof(Dictionary<CampaignDescriptor.DescriptorAttribute, object>));
        }
    }
}