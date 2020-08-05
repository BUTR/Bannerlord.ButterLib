using System.Collections.Generic;
using TaleWorlds.SaveSystem;
using CampaignIdentifier.CampaignBehaviors.BehaviorManagers;

namespace CampaignIdentifier
{
  internal class CISaveableTypeDefiner : SaveableTypeDefiner
  {
    public CISaveableTypeDefiner() : base(2011750000) { }
    protected override void DefineClassTypes()
    {
      base.AddClassDefinition(typeof(CampaignDescriptor), 1);
      base.AddClassDefinition(typeof(CampaignDescriptorManager), 10);
    }

    protected override void DefineEnumTypes()
    {
      base.AddEnumDefinition(typeof(CampaignDescriptor.DescriptorAttribute), 100);
    }

    protected override void DefineContainerDefinitions()
    {
      base.ConstructContainerDefinition(typeof(Dictionary<CampaignDescriptor.DescriptorAttribute, object>));
    }
  }
}
