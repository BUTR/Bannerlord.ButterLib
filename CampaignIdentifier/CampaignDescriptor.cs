using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using CampaignIdentifier.Helpers;
using TaleWorlds.SaveSystem;

namespace CampaignIdentifier
{
  [Serializable]
  [SaveableClass(1)]
  public sealed class CampaignDescriptor : ISerializable
  {
    //Consts
    private const int descriptorKeyLength = 12;
    private const string localizableValue = "{=UdgidGXLwg}{BASE_DESCRIPTOR_HERO.NAME}{?BASE_DESCRIPTOR_HERO.CLAN} {BASE_DESCRIPTOR_HERO.CLAN_NAME}{?}{\\?}{?BASE_DESCRIPTOR_HERO.BIRTHPLACE} from {BASE_DESCRIPTOR_HERO.BIRTHPLACE_NAME}{?}, birthplace unknown{\\?}, " +
                                            "{BASE_DESCRIPTOR_HERO.AGE}, {?BASE_DESCRIPTOR_HERO_PARENTS}{BASE_DESCRIPTOR_HERO_PARENTS.INFO}{?}parents unknown{\\?}.{?BASE_DESCRIPTOR_HERO.CULTURE} {BASE_DESCRIPTOR_HERO.CULTURE_NAME}.{?}{\\?}";
    private const string localizableParentsInfo = "{=svZkgB1GgB}{?BASE_DESCRIPTOR_HERO.GENDER}daughter{?}son{\\?} of {BASE_DESCRIPTOR_HERO.PARENTS_BEGIN}{?BASE_DESCRIPTOR_HERO.FULL_FAMILY} and {?}{\\?}{BASE_DESCRIPTOR_HERO.PARENTS_END}";

    //Static fields
    [NonSerialized]
    private static readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    //Fields
    [SaveableField(1)]
    private readonly string value;
    [SaveableField(2)]
    private readonly Dictionary<DescriptorAttribute, object> attributes;

    [NonSerialized]
    private readonly Hero baseHero;

    //Properties
    public string KeyValue => value;
    public string ImmutableKey => Regex.Replace(string.Format("GDR_{0}-BPL_{1}-CLR_{2}", attributes[DescriptorAttribute.HeroGender], attributes[DescriptorAttribute.BirthplaceName], attributes[DescriptorAttribute.HeroCulture]), @"\{[\\?=][^}]*\}", string.Empty);
    public string FullCharacterName => string.Join(" ", attributes[DescriptorAttribute.HeroName], attributes[DescriptorAttribute.HeroFamilyName]);
    public string Descriptor => GetLocalizableValue();
    public CharacterCode CharacterCode => CharacterCode.CreateFrom((string)attributes[DescriptorAttribute.CharacterCode]);

    //Constructors
    public CampaignDescriptor(Hero baseHero)
    {
      this.baseHero = baseHero;
      GetDescriptorByHero(out value, out attributes);
    }

    internal CampaignDescriptor(string value, Hero baseHero)
    {
      this.baseHero = baseHero;
      this.value = value;
      GetDescriptorByHero(out string _, out attributes);      
    }

    internal CampaignDescriptor(string value, Dictionary<DescriptorAttribute, object> attributes)
    {
      this.value = value;
      this.attributes = attributes;
      baseHero = null;
    }

    internal CampaignDescriptor(SerializationInfo info, StreamingContext context)
    {
      value = info.GetString(nameof(KeyValue));
      attributes = (Dictionary<DescriptorAttribute, object>)info.GetValue("DecriptorAttributes", typeof(Dictionary<DescriptorAttribute, object>));
      baseHero = null;
    }

    //Public methods
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      info.AddValue(nameof(KeyValue), value);
      info.AddValue("DecriptorAttributes", attributes);
    }

    //Private methods
    private void GetDescriptorByHero(out string value, out Dictionary<DescriptorAttribute, object> attributes)
    {
      attributes = Enum.GetValues(typeof(DescriptorAttribute)).Cast<DescriptorAttribute>().ToDictionary(key => key, val => GetDescriptorInfoPiece(val));
      value = GetUniqueKey(descriptorKeyLength);
    }

    private object GetDescriptorInfoPiece(DescriptorAttribute descriptorAttribute)
    {
      switch (descriptorAttribute)
      {
        case DescriptorAttribute.HeroName: return FieldAccessHelper.TextObjectValueByRef(baseHero.Name);
        case DescriptorAttribute.HeroFamilyName: return baseHero.Clan != null ? FieldAccessHelper.TextObjectValueByRef(baseHero.Clan.Name) : string.Empty;
        case DescriptorAttribute.HeroAge: return (int)baseHero.Age;
        case DescriptorAttribute.HeroGender: return baseHero.IsFemale ? 1 : 0;
        case DescriptorAttribute.HeroCulture: return baseHero.Culture != null ? FieldAccessHelper.TextObjectValueByRef(baseHero.Culture.Name) : string.Empty;
        case DescriptorAttribute.FatherName: return baseHero.Father != null ? FieldAccessHelper.TextObjectValueByRef(baseHero.Father.Name) : string.Empty;
        case DescriptorAttribute.MotherName: return baseHero.Mother != null ? FieldAccessHelper.TextObjectValueByRef(baseHero.Mother.Name) : string.Empty;
        case DescriptorAttribute.BirthplaceName: return baseHero.BornSettlement != null ? FieldAccessHelper.TextObjectValueByRef(baseHero.BornSettlement.Name) : string.Empty;
        case DescriptorAttribute.CharacterCode: return CharacterCode.CreateFrom(baseHero.CharacterObject).Code;
        default:
          throw DebugHelper.GetOutOfRangeException(descriptorAttribute, nameof(GetDescriptorInfoPiece), nameof(descriptorAttribute));
      }
    }

    private string GetLocalizableValue()
    {
      string familyName = (string)attributes[DescriptorAttribute.HeroFamilyName];
      string birthPlace = (string)attributes[DescriptorAttribute.BirthplaceName];
      string fatherName = (string)attributes[DescriptorAttribute.FatherName];
      string motherName = (string)attributes[DescriptorAttribute.MotherName];
      string cultureName = (string)attributes[DescriptorAttribute.HeroCulture];
      cultureName = Regex.Replace(cultureName, @"\{[\\?=][^}]*\}", string.Empty);

      TextObject resultTextObject = new TextObject(localizableValue);
      TextObject parentsInfoTextObject = new TextObject(localizableParentsInfo);

      //BASE_DESCRIPTOR_HERO
      TextObject baseHeroTextObject = new TextObject();
      baseHeroTextObject.SetTextVariable("NAME", (string)attributes[DescriptorAttribute.HeroName]);
      baseHeroTextObject.SetTextVariable("CLAN", familyName.Length > 0 ? 1 : 0);
      baseHeroTextObject.SetTextVariable("CLAN_NAME", familyName);
      baseHeroTextObject.SetTextVariable("AGE", (int)attributes[DescriptorAttribute.HeroAge]);
      baseHeroTextObject.SetTextVariable("GENDER", (int)attributes[DescriptorAttribute.HeroGender]);
      
      baseHeroTextObject.SetTextVariable("BIRTHPLACE", birthPlace.Length > 0 ? 1 : 0);
      baseHeroTextObject.SetTextVariable("BIRTHPLACE_NAME", birthPlace);

      baseHeroTextObject.SetTextVariable("CULTURE", cultureName.Length > 0 ? 1 : 0);
      baseHeroTextObject.SetTextVariable("CULTURE_NAME", GameTexts.FindText("str_adjective_for_faction", cultureName.ToLower()));

      if (fatherName.Length > 0 && motherName.Length > 0)
      {
        baseHeroTextObject.SetTextVariable("FULL_FAMILY", 1);
        baseHeroTextObject.SetTextVariable("PARENTS_BEGIN", fatherName);
        baseHeroTextObject.SetTextVariable("PARENTS_END", motherName);
      }
      else
      {
        baseHeroTextObject.SetTextVariable("FULL_FAMILY", 0);
        baseHeroTextObject.SetTextVariable("PARENTS_BEGIN", "");
        baseHeroTextObject.SetTextVariable("PARENTS_END", fatherName.Length > 0 ? fatherName : motherName);
      }

      resultTextObject.SetTextVariable("BASE_DESCRIPTOR_HERO", baseHeroTextObject);
      MBTextManager.SetTextVariable("BASE_DESCRIPTOR_HERO", baseHeroTextObject);

      //BASE_DESCRIPTOR_HERO_PARENTS
      TextObject parentsTextObject = new TextObject(fatherName.Length > 0 || motherName.Length > 0 ? 1 : 0);
      parentsTextObject.SetTextVariable("INFO", parentsInfoTextObject);
      resultTextObject.SetTextVariable("BASE_DESCRIPTOR_HERO_PARENTS", parentsTextObject);

      //Result
      return resultTextObject.ToString();
    }

    private static string GetUniqueKey(int size)
    {
      byte[] data = new byte[4 * size];
      using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
      {
        crypto.GetBytes(data);
      }
      StringBuilder result = new StringBuilder(size);
      for (int i = 0; i < size; i++)
      {
        var rnd = BitConverter.ToUInt32(data, i * 4);
        var idx = rnd % chars.Length;

        result.Append(chars[idx]);
      }
      return result.ToString();
    }

    [SaveableEnum(100)]
    internal enum DescriptorAttribute: byte
    {
      HeroName = 0,
      HeroFamilyName = 1,
      HeroAge = 2,
      HeroGender = 3,
      HeroCulture = 4,
      FatherName = 5,
      MotherName = 6,
      BirthplaceName = 7,
      CharacterCode = 8
    }
  }
}
