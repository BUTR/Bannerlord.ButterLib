using TaleWorlds.CampaignSystem;

namespace CampaignIdentifier
{
  public class CampaignIdentifierEvents
  {
    private readonly MbEvent _onDescriptorRelatedDataChanged = new MbEvent();
    private readonly MbEvent<CampaignDescriptor> _onDecriptorAssigned = new MbEvent<CampaignDescriptor>();

    public static CampaignIdentifierEvents Instance { get; internal set; }

    public static void RemoveListeners(object o)
    {
      Instance.RemoveListenersInternal(o);
    }

    internal void RemoveListenersInternal(object obj)
    {
      _onDescriptorRelatedDataChanged.ClearListeners(obj);
      _onDecriptorAssigned.ClearListeners(obj);
    }

    public static IMbEvent OnDescriptorRelatedDataChangedEvent => Instance._onDescriptorRelatedDataChanged;
    internal void OnDescriptorRelatedDataChanged()
    {
      Instance._onDescriptorRelatedDataChanged.Invoke();
    }

    public static IMbEvent<CampaignDescriptor> OnDecriptorAssignedEvent => Instance._onDecriptorAssigned;
    internal void OnDecriptorAssigned(CampaignDescriptor assignedDescriptor)
    {
      Instance._onDecriptorAssigned.Invoke(assignedDescriptor);
    }
  }
}
