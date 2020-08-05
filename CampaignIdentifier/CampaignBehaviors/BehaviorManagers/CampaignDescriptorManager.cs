using System;
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

namespace CampaignIdentifier.CampaignBehaviors.BehaviorManagers
{
  [SaveableClass(10)]
  public class CampaignDescriptorManager
  {
    private const string inquiryUpperBody = "{=gRz8MZ0YZ7}This game has no ID. It probably was started before installing Campaign Identifier. We have found other ongoing campaigns that have similar initial character background and were already assigned an ID. If this game refers to one of them, please, select corresponding option.";
    private const string inquiryLowerBody = "{=zed49rdkQR}Select '{NEW_ID}' if the loaded save is a separate campaign that has nothing to do with the suggested options and a new ID should be assigned to it.";

    [SaveableField(1)]
    private CampaignDescriptor _campaignDescriptor;

    private CampaignDescriptor _descriptorToBeAssigned = null;

    public static readonly string ExistingCampaignDescriptorsLogFile = Path.Combine(Utilities.GetConfigsPath(), "CampaignIdentifier", "ExistingCampaignIdentifiers.bin");

    internal CampaignDescriptorManager()
    {
      _campaignDescriptor = null;
    }

    public CampaignDescriptor CampaignDescriptor => _campaignDescriptor;

    internal void GenerateNewGameDescriptor()
    {
      _campaignDescriptor = new CampaignDescriptor(Hero.MainHero);
      CampaignIdentifierEvents.Instance.OnDecriptorAssigned(_campaignDescriptor);
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
      _descriptorToBeAssigned = Campaign.Current.GameStarted && Hero.All.FirstOrDefault(h => h.Id.SubId == 1) is Hero baseHero ? new CampaignDescriptor(baseHero) : null;
      if (_descriptorToBeAssigned is null)
      {
        return;
      }

      List<CampaignDescriptor> PossiblyMatchingDescriptors = LoadExistingDescriptors().Where(cd => cd.ImmutableKey == _descriptorToBeAssigned.ImmutableKey).ToList();
      if (PossiblyMatchingDescriptors.Any())
      {
        List<InquiryElement> inquiryElements = new List<InquiryElement>();
        foreach (CampaignDescriptor existingDescriptor in PossiblyMatchingDescriptors)
        {
          inquiryElements.Add(new InquiryElement(existingDescriptor, existingDescriptor.FullCharacterName, new ImageIdentifier(existingDescriptor.CharacterCode), true, string.Join(" - ", existingDescriptor.Descriptor, existingDescriptor.KeyValue)));
        }
        TextObject newIDTextObject = new TextObject("{=wF4qRrhmEu}Assign new ID");
        string inquiryBody = new TextObject(inquiryUpperBody).ToString() + "\n \n" + new TextObject(inquiryLowerBody, new Dictionary<string, TextObject>() { ["NEW_ID"] = newIDTextObject }).ToString();
        inquiryElements.Add(new InquiryElement(null, newIDTextObject.ToString(), new ImageIdentifier(ImageIdentifierType.Null), true, new TextObject("{=25Ts3iQnv6}This is a standalone campaign and must be assigned a new ID.").ToString()));
        InformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=4BuIRaILsb}Select identified ongoing campaign").ToString(),
                                                                                   inquiryBody, inquiryElements, true, 1, GameTexts.FindText("str_done", null).ToString(), "",
                                                                                   new Action<List<InquiryElement>>(OnDescriptorSelectionOver),
                                                                                   new Action<List<InquiryElement>>(OnDescriptorSelectionOver),
                                                                                   ""), false);
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
      CampaignIdentifierEvents.Instance.OnDecriptorAssigned(_campaignDescriptor);
    }

    private void UpdateSavedDescriptors()
    {
      List<CampaignDescriptor> ExistingCampaignDescriptors = LoadExistingDescriptors();
      ExistingCampaignDescriptors.Add(_campaignDescriptor);
      if (!Directory.Exists(Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile)))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(ExistingCampaignDescriptorsLogFile));
      }
      using (FileStream fileStream = File.OpenWrite(ExistingCampaignDescriptorsLogFile))
      {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, ExistingCampaignDescriptors);
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
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        List<CampaignDescriptor> ExistingCampaignDescriptors = (List<CampaignDescriptor>)binaryFormatter.Deserialize(fileStream);
        return ExistingCampaignDescriptors;
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
