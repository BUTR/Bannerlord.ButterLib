using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    public class CampaignIdentifierEvents
    {
        private readonly MbEvent _onDescriptorRelatedDataChanged = new MbEvent();
        private readonly MbEvent<CampaignDescriptor> _onDescriptorAssigned = new MbEvent<CampaignDescriptor>();

        public static CampaignIdentifierEvents Instance { get; internal set; } = null!; // Won't be null when properly accessed.

        public static void RemoveListeners(object o)
        {
            Instance.RemoveListenersInternal(o);
        }

        internal void RemoveListenersInternal(object obj)
        {
            _onDescriptorRelatedDataChanged.ClearListeners(obj);
            _onDescriptorAssigned.ClearListeners(obj);
        }

        public static IMbEvent OnDescriptorRelatedDataChangedEvent => Instance._onDescriptorRelatedDataChanged;

        internal void OnDescriptorRelatedDataChanged()
        {
            Instance._onDescriptorRelatedDataChanged.Invoke();
        }

        public static IMbEvent<CampaignDescriptor> OnDescriptorAssignedEvent => Instance._onDescriptorAssigned;

        internal void OnDescriptorAssigned(CampaignDescriptor assignedDescriptor)
        {
            Instance._onDescriptorAssigned.Invoke(assignedDescriptor);
        }
    }
}