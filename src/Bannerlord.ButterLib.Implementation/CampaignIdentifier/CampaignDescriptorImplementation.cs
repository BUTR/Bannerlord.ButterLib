using Bannerlord.ButterLib.CampaignIdentifier;
using Bannerlord.ButterLib.Implementation.CampaignIdentifier.Helpers;

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
using TaleWorlds.SaveSystem;

namespace Bannerlord.ButterLib.Implementation.CampaignIdentifier
{
    /// <summary>Sealed class that is used to assign and hold campaign alphanumeric ID.</summary>
    /// <remarks>
    /// Also stores some general description on the campaign, based on a specified hero
    /// (default is the initial player character in the current campaign).
    /// </remarks>
    [Serializable]
    [SaveableClass(1)]
    internal sealed class CampaignDescriptorImplementation : CampaignDescriptor, ISerializable
    {
        //Consts
        private const int DescriptorKeyLength = 12;

        private const string LocalizableValue =
            "{=UdgidGXLwg}{BASE_DESCRIPTOR_HERO.NAME}{?BASE_DESCRIPTOR_HERO.CLAN} {BASE_DESCRIPTOR_HERO.CLAN_NAME}{?}{\\?}{?BASE_DESCRIPTOR_HERO.BIRTHPLACE} from {BASE_DESCRIPTOR_HERO.BIRTHPLACE_NAME}{?}, birthplace unknown{\\?}, " +
            "{BASE_DESCRIPTOR_HERO.AGE}, {?BASE_DESCRIPTOR_HERO_PARENTS}{BASE_DESCRIPTOR_HERO_PARENTS.INFO}{?}parents unknown{\\?}.{?BASE_DESCRIPTOR_HERO.CULTURE} {BASE_DESCRIPTOR_HERO.CULTURE_NAME}.{?}{\\?}";

        private const string LocalizableParentsInfo =
            "{=svZkgB1GgB}{?BASE_DESCRIPTOR_HERO.GENDER}daughter{?}son{\\?} of {BASE_DESCRIPTOR_HERO.PARENTS_BEGIN}{?BASE_DESCRIPTOR_HERO.FULL_FAMILY} and {?}{\\?}{BASE_DESCRIPTOR_HERO.PARENTS_END}";

        //Static fields
        [NonSerialized]
        private static readonly char[] Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

        //Fields
        [SaveableField(1)]
        private readonly string _value;
        [SaveableField(2)]
        private readonly Dictionary<DescriptorAttribute, object> _attributes;

        [NonSerialized]
        private readonly Hero _baseHero;

        //Properties
        /// <summary>Alphanumeric campaign ID.</summary>
        public override string KeyValue => _value;

        /// <summary>A string key that is used to compare unidentified ongoing campaigns.</summary>
        /// <remarks>Contains information about gender, birthplace and culture of the initial character.</remarks>
        public override string ImmutableKey => Regex.Replace(
            $"GDR_{_attributes[DescriptorAttribute.HeroGender]}-BPL_{_attributes[DescriptorAttribute.BirthplaceName]}-CLR_{_attributes[DescriptorAttribute.HeroCulture]}",
            @"\{[\\?=][^}]*\}", string.Empty);

        /// <summary>
        /// Name of the <see cref="T:TaleWorlds.CampaignSystem.Hero" />
        /// that was used when creating this <see cref="T:Bannerlord.ButterLib.CampaignIdentifier.CampaignDescriptor" /> instance.
        /// </summary>
        public override string FullCharacterName => string.Join(" ", _attributes[DescriptorAttribute.HeroName], _attributes[DescriptorAttribute.HeroFamilyName]);

        /// <summary>
        /// Localizable description of the campaign, based on the <see cref="Hero" /> 
        /// that was used when creating this <see cref="CampaignDescriptorImplementation" /> instance.
        /// </summary>
        public override string Descriptor => GetLocalizableValue();

        /// <summary>
        /// The <see cref="T:TaleWorlds.Core.CharacterCode" /> of the <see cref="Hero" /> 
        /// that current <see cref="CampaignDescriptorImplementation" /> instance is based on.
        /// </summary>
        public override CharacterCode CharacterCode => CharacterCode.CreateFrom((string) _attributes[DescriptorAttribute.CharacterCode]);

        //Constructors
        /// <summary>Initializes a new instance of the class <see cref="CampaignDescriptorImplementation" />.</summary>
        /// <param name="baseHero">The hero to be used as descriptor base.</param>
        public CampaignDescriptorImplementation(Hero baseHero)
        {
            _baseHero = baseHero;
            GetDescriptorByHero(out _value, out _attributes);
        }

        internal CampaignDescriptorImplementation(string value, Hero baseHero)
        {
            _baseHero = baseHero;
            _value = value;
            GetDescriptorByHero(out string _, out _attributes);
        }

        internal CampaignDescriptorImplementation(string value, Dictionary<DescriptorAttribute, object> attributes)
        {
            _value = value;
            _attributes = attributes;
            _baseHero = null!; // Won't be needed since we have the attributes already
        }

        private CampaignDescriptorImplementation(SerializationInfo info, StreamingContext context)
        {
            _value = info.GetString(nameof(KeyValue));
            _attributes = (Dictionary<DescriptorAttribute, object>) info.GetValue("DecriptorAttributes", typeof(Dictionary<DescriptorAttribute, object>));
            _baseHero = null!; // Serialization will do it's thing.
        }

        //Public methods
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(KeyValue), _value);
            info.AddValue("DecriptorAttributes", _attributes);
        }

        //Private methods
        private void GetDescriptorByHero(out string value, out Dictionary<DescriptorAttribute, object> attributes)
        {
            attributes = Enum.GetValues(typeof(DescriptorAttribute))
                .Cast<DescriptorAttribute>()
                .ToDictionary(key => key, GetDescriptorInfoPiece);
            value = GetUniqueKey(DescriptorKeyLength);
        }

        private object GetDescriptorInfoPiece(DescriptorAttribute descriptorAttribute) => descriptorAttribute switch
        {
            DescriptorAttribute.HeroName => FieldAccessHelper.TextObjectValueByRef(_baseHero.Name),
            DescriptorAttribute.HeroFamilyName => _baseHero.Clan != null
                ? FieldAccessHelper.TextObjectValueByRef(_baseHero.Clan.Name)
                : string.Empty,
            DescriptorAttribute.HeroAge => (int) _baseHero.Age,
            DescriptorAttribute.HeroGender => _baseHero.IsFemale
                ? 1
                : 0,
            DescriptorAttribute.HeroCulture => _baseHero.Culture != null
                ? FieldAccessHelper.TextObjectValueByRef(_baseHero.Culture.Name)
                : string.Empty,
            DescriptorAttribute.FatherName => _baseHero.Father != null
                ? FieldAccessHelper.TextObjectValueByRef(_baseHero.Father.Name)
                : string.Empty,
            DescriptorAttribute.MotherName => _baseHero.Mother != null
                ? FieldAccessHelper.TextObjectValueByRef(_baseHero.Mother.Name)
                : string.Empty,
            DescriptorAttribute.BirthplaceName => _baseHero.BornSettlement != null
                ? FieldAccessHelper.TextObjectValueByRef(_baseHero.BornSettlement.Name)
                : string.Empty,
            DescriptorAttribute.CharacterCode => CharacterCode.CreateFrom(_baseHero.CharacterObject).Code,
            _ => throw DebugHelper.GetOutOfRangeException(descriptorAttribute, nameof(GetDescriptorInfoPiece), nameof(descriptorAttribute))
        };

        private string GetLocalizableValue()
        {
            var familyName = (string) _attributes[DescriptorAttribute.HeroFamilyName];
            var birthPlace = (string) _attributes[DescriptorAttribute.BirthplaceName];
            var fatherName = (string) _attributes[DescriptorAttribute.FatherName];
            var motherName = (string) _attributes[DescriptorAttribute.MotherName];
            var cultureName = (string) _attributes[DescriptorAttribute.HeroCulture];
            cultureName = Regex.Replace(cultureName, @"\{[\\?=][^}]*\}", string.Empty);

            var resultTextObject = new TextObject(LocalizableValue);
            var parentsInfoTextObject = new TextObject(LocalizableParentsInfo);

            //BASE_DESCRIPTOR_HERO
            var baseHeroTextObject = new TextObject();
            baseHeroTextObject.SetTextVariable("NAME", (string) _attributes[DescriptorAttribute.HeroName]);
            baseHeroTextObject.SetTextVariable("CLAN", familyName.Length > 0 ? 1 : 0);
            baseHeroTextObject.SetTextVariable("CLAN_NAME", familyName);
            baseHeroTextObject.SetTextVariable("AGE", (int) _attributes[DescriptorAttribute.HeroAge]);
            baseHeroTextObject.SetTextVariable("GENDER", (int) _attributes[DescriptorAttribute.HeroGender]);

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
            var parentsTextObject = new TextObject(fatherName.Length > 0 || motherName.Length > 0 ? 1 : 0);
            parentsTextObject.SetTextVariable("INFO", parentsInfoTextObject);
            resultTextObject.SetTextVariable("BASE_DESCRIPTOR_HERO_PARENTS", parentsTextObject);

            //Result
            return resultTextObject.ToString();
        }

        private static string GetUniqueKey(int size)
        {
            var data = new byte[4 * size];
            using var crypto = new RNGCryptoServiceProvider();
            crypto.GetBytes(data);

            var result = new StringBuilder(size);
            for (var i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % Chars.Length;

                result.Append(Chars[idx]);
            }

            return result.ToString();
        }

        [SaveableEnum(3)]
        internal enum DescriptorAttribute : byte
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