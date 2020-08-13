using Bannerlord.ButterLib.CampaignIdentifier;

using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    /// <summary>Custom events used by CampaignIdentifier.</summary>
    internal class CampaignIdentifierEventsImplementation : CampaignIdentifierEvents
    {
        protected override MbEvent _onDescriptorRelatedDataChanged { get; } = new MbEvent();
        protected override MbEvent<CampaignDescriptor> _onDescriptorAssigned { get; } = new MbEvent<CampaignDescriptor>();

        public override void OnDescriptorRelatedDataChanged()
        {
            _onDescriptorRelatedDataChanged.Invoke();
        }

        protected override void RemoveListenersInternal(object obj)
        {
            _onDescriptorRelatedDataChanged.ClearListeners(obj);
            _onDescriptorAssigned.ClearListeners(obj);
        }

        internal void OnDescriptorAssigned(CampaignDescriptorImplementation assignedDescriptor)
        {
            _onDescriptorAssigned.Invoke(assignedDescriptor);
        }
    }
}