using TaleWorlds.CampaignSystem;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    /// <summary>Custom events used by CampaignIdentifier.</summary>
    public sealed class CampaignIdentifierEvents
    {
        private readonly MbEvent _onDescriptorRelatedDataChanged = new MbEvent();
        private readonly MbEvent<CampaignDescriptor> _onDescriptorAssigned = new MbEvent<CampaignDescriptor>();

        /// <summary>An instance of the CampaignIdentifier custom events.</summary>
        /// <remarks>Assigned in the process of creating new game or loading existing one.</remarks>
        public static CampaignIdentifierEvents Instance { get; internal set; } = null!; // Won't be null when properly accessed.

        /// <summary>
        /// Removes any listeners to the <see cref="CampaignIdentifierEvents" /> 
        /// that are associated with a certain <see cref="T:System.Object" />.
        /// </summary>
        /// <param name="o">The object holding the listeners that are to be removed</param>
        /// <remarks>
        /// This is primarily used to deactivate certain behaviors. 
        /// In that case argument "o" should be assigned the behavior to be deactivated.
        /// </remarks>
        public static void RemoveListeners(object o)
        {
            Instance.RemoveListenersInternal(o);
        }

        internal void RemoveListenersInternal(object obj)
        {
            _onDescriptorRelatedDataChanged.ClearListeners(obj);
            _onDescriptorAssigned.ClearListeners(obj);
        }

        /// <summary>
        /// A custom event indicating a change in <see cref="CampaignDescriptor" /> information.
        /// </summary>
        /// <remarks>
        /// This event is used to update information in the <see cref="CampaignDescriptor" /> assigned to the campaign.
        /// It should be fired when initial player character's gender, culture or birthplace are changed.
        /// </remarks>
        public static IMbEvent OnDescriptorRelatedDataChangedEvent => Instance._onDescriptorRelatedDataChanged;

        /// <summary>Fires OnDescriptorRelatedDataChangedEvent.</summary>
        /// <remarks>
        /// Said event should be raised when there are changes to the information stored in <see cref="CampaignDescriptor" />.
        /// Such information is: culture, gender, name, age, birthplace and names of parents of the initial player character, as well as player's clan name.
        /// Note that only gender, culture and birthplace are really determinative ones, others are just for the reference and changes to them could be safely ignored.
        /// </remarks>
        public void OnDescriptorRelatedDataChanged()
        {
            Instance._onDescriptorRelatedDataChanged.Invoke();
        }

        /// <summary>A custom event that fires when a campaign is assigned an ID.</summary>
        /// <remarks>The assigned ID would be stored in the event argument.</remarks>
        public static IMbEvent<CampaignDescriptor> OnDescriptorAssignedEvent => Instance._onDescriptorAssigned;

        internal void OnDescriptorAssigned(CampaignDescriptor assignedDescriptor)
        {
            Instance._onDescriptorAssigned.Invoke(assignedDescriptor);
        }
    }
}