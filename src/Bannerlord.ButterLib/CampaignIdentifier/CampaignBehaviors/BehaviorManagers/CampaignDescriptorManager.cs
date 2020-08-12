using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

using Path = System.IO.Path;

namespace Bannerlord.ButterLib.CampaignIdentifier.CampaignBehaviors.BehaviorManagers
{
    [SaveableClass(2)]
    internal class CampaignDescriptorManager
    {
        private const string InquiryUpperBody =
            "{=gRz8MZ0YZ7}This game has no ID. It probably was started before installing Campaign Identifier. We have found other ongoing campaigns that have similar initial character background and were already assigned an ID. If this game refers to one of them, please, select corresponding option.";

        private const string InquiryLowerBody =
            "{=zed49rdkQR}Select '{NEW_ID}' if the loaded save is a separate campaign that has nothing to do with the suggested options and a new ID should be assigned to it.";

        [SaveableField(1)]
        private CampaignDescriptor _campaignDescriptor;

        private CampaignDescriptor? _descriptorToBeAssigned;

        internal static readonly string ExistingCampaignDescriptorsLogFile = Path.Combine(Utilities.GetConfigsPath(), "ButterLib", "CampaignIdentifier", "ExistingCampaignIdentifiers.bin");

        internal CampaignDescriptorManager()
        {
            _campaignDescriptor = null!; // Won't be null when properly accessed.
        }

        internal CampaignDescriptor CampaignDescriptor => _campaignDescriptor;

        internal void GenerateNewGameDescriptor()
        {
            _campaignDescriptor = new CampaignDescriptor(Hero.MainHero);
            CampaignIdentifierEvents.Instance.OnDescriptorAssigned(_campaignDescriptor);
        }

        internal void CheckCampaignDescriptor()
        {
            if (_campaignDescriptor is null)
            {
                AddDescriptorToExistingCampaign();
            }
        }

        internal void UpdateCampaignDescriptor(Hero baseHero)
        {
            _campaignDescriptor = new CampaignDescriptor(_campaignDescriptor.KeyValue, baseHero);
        }

        private void AddDescriptorToExistingCampaign()
        {
            _descriptorToBeAssigned = Campaign.Current.GameStarted && Hero.All.FirstOrDefault(h => h.Id.SubId == 1) is { } baseHero
                ? new CampaignDescriptor(baseHero)
                : null;
            if (_descriptorToBeAssigned is null)
            {
                return;
            }

            var possiblyMatchingDescriptors = LoadExistingDescriptors()
                .Where(cd => cd.ImmutableKey == _descriptorToBeAssigned.ImmutableKey)
                .ToList();

            if (possiblyMatchingDescriptors.Any())
            {
                var inquiryElements = possiblyMatchingDescriptors
                    .Select(existingDescriptor => new InquiryElement(
                        existingDescriptor,
                        existingDescriptor.FullCharacterName,
                        new ImageIdentifier(existingDescriptor.CharacterCode),
                        true,
                        string.Join(" - ", existingDescriptor.Descriptor, existingDescriptor.KeyValue)))
                    .ToList();

                var newIdTextObject = new TextObject("{=wF4qRrhmEu}Assign new ID");
                var inquiryBody = $"{new TextObject(InquiryUpperBody)}\n \n{new TextObject(InquiryLowerBody, new Dictionary<string, TextObject> {["NEW_ID"] = newIdTextObject})}";
                inquiryElements.Add(new InquiryElement(null, newIdTextObject.ToString(),
                    new ImageIdentifier(), true,
                    new TextObject("{=25Ts3iQnv6}This is a standalone campaign and must be assigned a new ID.").ToString()));
                InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(
                    new TextObject("{=4BuIRaILsb}Select identified ongoing campaign").ToString(),
                    inquiryBody, inquiryElements, true, 1, GameTexts.FindText("str_done").ToString(), "",
                    OnDescriptorSelectionOver,
                    OnDescriptorSelectionOver));
                return;
            }

            ApplyPredeterminedDescriptor();
        }

        private void OnDescriptorSelectionOver(List<InquiryElement> element)
        {
            if (element.Count > 0 && element[0].Identifier is CampaignDescriptor chosenDescriptor)
            {
                _descriptorToBeAssigned = chosenDescriptor;
            }

            ApplyPredeterminedDescriptor();
        }

        private void ApplyPredeterminedDescriptor()
        {
            if (_descriptorToBeAssigned is null)
            {
                return;
            }

            _campaignDescriptor = _descriptorToBeAssigned;
            CampaignIdentifierEvents.Instance.OnDescriptorAssigned(_campaignDescriptor);
        }

        private void UpdateSavedDescriptors()
        {
            var existingCampaignDescriptors = LoadExistingDescriptors();
            existingCampaignDescriptors.Add(_campaignDescriptor);
            if (!Directory.Exists(Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile));
            }

            using (FileStream fileStream = File.OpenWrite(ExistingCampaignDescriptorsLogFile))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, existingCampaignDescriptors);
            }
        }

        private static List<CampaignDescriptor> LoadExistingDescriptors()
        {
            if (!File.Exists(ExistingCampaignDescriptorsLogFile))
            {
                return new List<CampaignDescriptor>();
            }

            using (FileStream fileStream = File.OpenRead(ExistingCampaignDescriptorsLogFile))
            {
                var binaryFormatter = new BinaryFormatter();
                var existingCampaignDescriptors = (List<CampaignDescriptor>) binaryFormatter.Deserialize(fileStream);
                return existingCampaignDescriptors;
            }
        }

        internal void Sync()
        {
            if (_descriptorToBeAssigned != null && _campaignDescriptor != null && _descriptorToBeAssigned == _campaignDescriptor)
            {
                UpdateSavedDescriptors();
            }

            _descriptorToBeAssigned = null;
        }
    }
}