using Bannerlord.BUTR.Shared.Helpers;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

// ReSharper disable once CheckNamespace
namespace Bannerlord.ButterLib.Common.Helpers
{
    /// <summary>Helper class used to store numeric variables or complex entities in the <see cref="TextObject" />.</summary>
    public static class LocalizationHelper
    {
        /// <summary>
        /// A string tag that corresponds with <see cref="PluralForm.Plural" />
        /// and could be stored as an attribute of the numeric variable by the "SetNumericVariable" method.
        /// </summary>
        public const string PLURAL_FORM_TAG = "PLURAL_FORM";

        /// <summary>
        /// A string tag that corresponds with <see cref="PluralForm.SpecificSingular" />
        /// and could be stored as an attribute of the numeric variable by the "SetNumericVariable" method.
        /// </summary>
        public const string SPECIFIC_SINGULAR_FORM_TAG = "SPECIFIC_SINGULAR_FORM";

        /// <summary>
        /// A string tag that corresponds with <see cref="PluralForm.SpecificPlural" />
        /// and could be stored as an attribute of the numeric variable by the "SetNumericVariable" method.
        /// </summary>
        public const string SPECIFIC_PLURAL_FORM_TAG = "SPECIFIC_PLURAL_FORM";

        private static readonly ReadOnlyCollection<int> EasternSlavicPluralExceptions = new(new List<int> { 11, 12, 13, 14 });
        private static readonly ReadOnlyCollection<int> EasternSlavicSingularNumerics = new(new List<int> { 1, 2, 3, 4 });

        private static readonly ReadOnlyCollection<int> WesternSlavicPluralExceptions = new(new List<int> { 12, 13, 14 });
        private static readonly ReadOnlyCollection<int> WesternSlavicSingularNumerics = new(new List<int> { 2, 3, 4 });

        private static readonly ReadOnlyCollection<string> EasternSlavicGroupLanguageIDs = new(new List<string> { "Russian", "Русский", "Ukrainian", "Українська", "Belarusian", "Беларускі" });
        private static readonly ReadOnlyCollection<string> WesternSlavicGroupLanguageIDs = new(new List<string> { "Polish", "Polski" });

        private static RecursiveCaller GetRecursiveCaller(RecursiveCaller currentCaller, RecursiveCaller receivedCaller)
        {
            return (RecursiveCaller)Math.Max((byte)currentCaller, (byte)receivedCaller);
        }

        private static RecursiveCaller GetCurrentCaller<T>(T entity) where T : class
        {
            return entity switch
            {
                Hero _ => RecursiveCaller.Hero,
                Settlement _ => RecursiveCaller.Settlement,
                Clan _ => RecursiveCaller.Clan,
                Kingdom _ => RecursiveCaller.Kingdom,
                _ => throw new ArgumentException($"{entity.GetType().FullName} is not supported type", nameof(entity)),
            };
        }

        private static TextObject GetEntityTextObject<T>(T entity) where T : class
        {
            switch (entity)
            {
                case Hero hero:
                    var characterProperties = TextObjectHelper.Create(string.Empty);
                    characterProperties!.SetTextVariable("NAME", hero.Name);
                    characterProperties.SetTextVariable("AGE", (int)hero.Age);
                    characterProperties.SetTextVariable("GENDER", hero.IsFemale ? 1 : 0);
                    characterProperties.SetTextVariable("LINK", hero.EncyclopediaLinkWithName);
                    characterProperties.SetTextVariable("FIRSTNAME", hero.FirstName ?? hero.Name);
                    return characterProperties;
                case Settlement settlement:
                    var settlementProperties = TextObjectHelper.Create(string.Empty);
                    settlementProperties!.SetTextVariable("NAME", settlement.Name);
                    settlementProperties.SetTextVariable("IS_TOWN", settlement.IsTown ? 1 : 0);
                    settlementProperties.SetTextVariable("IS_CASTLE", settlement.IsCastle ? 1 : 0);
                    settlementProperties.SetTextVariable("IS_VILLAGE", settlement.IsVillage ? 1 : 0);
                    settlementProperties.SetTextVariable("LINK", settlement.EncyclopediaLinkWithName);
                    return settlementProperties;
                case Clan clan:
                    var clanProperties = TextObjectHelper.Create(string.Empty);
                    clanProperties!.SetTextVariable("NAME", clan.Name);
                    clanProperties.SetTextVariable("MINOR_FACTION", clan.IsMinorFaction ? 1 : 0);
                    clanProperties.SetTextVariable("UNDER_CONTRACT", clan.IsUnderMercenaryService ? 1 : 0);
                    clanProperties.SetTextVariable("LINK", clan.EncyclopediaLinkWithName);
                    return clanProperties;
                case Kingdom kingdom:
                    var kingdomProperties = TextObjectHelper.Create(string.Empty);
                    kingdomProperties!.SetTextVariable("NAME", kingdom.Name);
                    kingdomProperties.SetTextVariable("LINK", kingdom.EncyclopediaLinkWithName);
                    return kingdomProperties;
                default:
                    throw new ArgumentException($"{entity.GetType().FullName} is not supported type", nameof(entity));
            }
        }

        private static void SetRelatedProperties<T>(TextObject? parentTextObject, string tag, T entity, bool addLeaderInfo, RecursiveCaller recursiveCaller) where T : class
        {
            switch (entity)
            {
                case Hero hero:
                    SetEntityProperties(parentTextObject, tag + "_CLAN", hero.Clan, addLeaderInfo, GetRecursiveCaller(RecursiveCaller.Hero, recursiveCaller));
                    break;
                case Settlement settlement:
                    SetEntityProperties(parentTextObject, tag + "_CLAN", settlement.OwnerClan, addLeaderInfo, GetRecursiveCaller(RecursiveCaller.Settlement, recursiveCaller));
                    break;
                case Clan clan:
                    if (addLeaderInfo)
                    {
                        SetEntityProperties(parentTextObject, tag + "_LEADER", clan.Leader, false, RecursiveCaller.Clan);
                    }
                    if (clan.Kingdom is not null)
                    {
                        SetEntityProperties(parentTextObject, tag + "_KINGDOM", clan.Kingdom, addLeaderInfo, GetRecursiveCaller(RecursiveCaller.Clan, recursiveCaller));
                    }
                    break;
                case Kingdom kingdom:
                    if (addLeaderInfo)
                    {
                        SetEntityProperties(parentTextObject, tag + "_LEADER", kingdom.Leader, false, RecursiveCaller.Kingdom);
                    }
                    break;
                default:
                    throw new ArgumentException($"{entity.GetType().FullName} is not supported type", nameof(entity));
            }
        }


        /// <summary>Sets complex entity into specified text variable, along with additional information on other related entities.</summary>
        /// <typeparam name="T">The type of the entity to be stored.</typeparam>
        /// <param name="parentTextObject">
        /// The <see cref="TextObject" /> to store entity information into.
        /// Null means that information will be stored into <see cref="MBTextManager" />.
        /// </param>
        /// <param name="tag">A string tag that will be used to store information about entity and also as a prefix for tags that will store other relevant entities.</param>
        /// <param name="entity">An instance of the entity to be stored.</param>
        /// <param name="addLeaderInfo">An optional argument, specifying if information about leaders should be also stored, when applicable.</param>
        /// <param name="recursiveCaller">An optional argument, specifying if method is called from itself, adding information on some related entity.</param>
        public static void SetEntityProperties<T>(TextObject? parentTextObject, string tag, T? entity, bool addLeaderInfo = false, RecursiveCaller recursiveCaller = RecursiveCaller.None) where T : class
        {
            if (string.IsNullOrEmpty(tag) || entity is null || recursiveCaller == GetCurrentCaller(entity))
            {
                return;
            }
            if (parentTextObject is null)
            {
                MBTextManager.SetTextVariable(tag, GetEntityTextObject(entity));
            }
            else
            {
                parentTextObject.SetTextVariable(tag, GetEntityTextObject(entity));
            }
            SetRelatedProperties(parentTextObject, tag, entity, addLeaderInfo, recursiveCaller);
        }

        private static PluralForm GetEasternSlavicPluralFormInternal(int number)
        {
            var absNumber = Math.Abs(number);
            var lastDigit = absNumber % 10;
            return
              EasternSlavicPluralExceptions.Contains(absNumber % 100) || !EasternSlavicSingularNumerics.Contains(lastDigit)
                ? PluralForm.Plural
                : !EasternSlavicPluralExceptions.Contains(absNumber) && EasternSlavicSingularNumerics.Contains(lastDigit) && lastDigit != 1
                ? PluralForm.SpecificSingular : PluralForm.Singular;
        }

        private static PluralForm GetWesternSlavicPluralFormInternal(int number)
        {
            var absNumber = Math.Abs(number);
            var lastDigit = absNumber % 10;
            return
              absNumber > 1 && (WesternSlavicPluralExceptions.Contains(absNumber % 100) || !WesternSlavicSingularNumerics.Contains(lastDigit))
                ? PluralForm.Plural
                : !WesternSlavicPluralExceptions.Contains(absNumber) && WesternSlavicSingularNumerics.Contains(lastDigit)
                ? PluralForm.SpecificPlural : PluralForm.Singular;
        }

        /// <summary>Gets which <see cref="PluralForm" /> should be used with the given number according to the grammar rules of the game language.</summary>
        /// <param name="number">An integer number to get appropriate <see cref="PluralForm" /> for.</param>
        /// <returns>The appropriate <see cref="PluralForm" /> that should be used with the given number in accordance with the grammar rules of the game language.</returns>
        public static PluralForm GetPluralForm(int number)
        {
            if (EasternSlavicGroupLanguageIDs.Contains(BannerlordConfig.Language))
            {
                return GetEasternSlavicPluralFormInternal(number);
            }
            if (WesternSlavicGroupLanguageIDs.Contains(BannerlordConfig.Language))
            {
                return GetWesternSlavicPluralFormInternal(number);
            }
            return number != 1 ? PluralForm.Plural : PluralForm.Singular;
        }

        /// <summary>Gets which <see cref="PluralForm" /> should be used with the given number according to the grammar rules of the game language.</summary>
        /// <param name="number">A floating-point number to get appropriate <see cref="PluralForm" /> for.</param>
        /// <returns>The appropriate <see cref="PluralForm" /> that should be used with the given number in accordance with the grammar rules of the game language.</returns>
        public static PluralForm GetPluralForm(float number)
        {
            if (EasternSlavicGroupLanguageIDs.Contains(BannerlordConfig.Language))
            {
                return GetEasternSlavicPluralFormInternal((int)Math.Floor(number));
            }
            return number != 1 ? PluralForm.Plural : PluralForm.Singular;
        }

        private static Dictionary<string, TextObject?> GetPluralFormAttributes(PluralForm pluralForm) =>
            new()
            {
                [PLURAL_FORM_TAG] = TextObjectHelper.Create(pluralForm == PluralForm.Plural ? 1 : 0),
                [SPECIFIC_PLURAL_FORM_TAG] = TextObjectHelper.Create(pluralForm != PluralForm.SpecificPlural ? 1 : 0),
                [SPECIFIC_SINGULAR_FORM_TAG] = TextObjectHelper.Create(pluralForm != PluralForm.SpecificSingular ? 1 : 0)
            };

        /// <summary>Sets a numeric variable along with the appropriate <see cref="PluralForm" /> tag in accordance with the grammar rules of the game language.</summary>
        /// <param name="textObject">
        /// The <see cref="TextObject" /> to set a numeric variable into.
        /// Null means that information will be stored into <see cref="MBTextManager" />.
        /// </param>
        /// <param name="tag">A string tag that will be used to store information about the numeric variable.</param>
        /// <param name="variableValue">An integer number to be set.</param>
        /// <param name="format">An optional argument, specifying string format to be used with the number.</param>
        public static void SetNumericVariable(TextObject? textObject, string tag, int variableValue, string? format = null)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }
            var attributes = GetPluralFormAttributes(GetPluralForm(variableValue));
            var explainedTextObject = string.IsNullOrEmpty(format)
                ? TextObjectHelper.Create(variableValue.ToString(), attributes)
                : TextObjectHelper.Create(variableValue.ToString(format), attributes);
            if (textObject is null)
            {
                MBTextManager.SetTextVariable(tag, explainedTextObject);
            }
            else
            {
                textObject.SetTextVariable(tag, explainedTextObject);
            }
        }

        /// <summary>Sets a numeric variable along with the appropriate <see cref="PluralForm" /> tag in accordance with the grammar rules of the game language.</summary>
        /// <param name="textObject">
        /// The <see cref="TextObject" /> to set a numeric variable into.
        /// Null means that information will be stored into <see cref="MBTextManager" />.
        /// </param>
        /// <param name="tag">A string tag that will be used to store information about the numeric variable.</param>
        /// <param name="variableValue">An floating-point number to be set.</param>
        /// <param name="format">An optional argument, specifying string format to be used with the number.</param>
        public static void SetNumericVariable(TextObject? textObject, string tag, float variableValue, string? format = null)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }
            var attributes = GetPluralFormAttributes(GetPluralForm(variableValue));
            var explainedTextObject = string.IsNullOrEmpty(format)
                ? TextObjectHelper.Create(variableValue.ToString("R"), attributes)
                : TextObjectHelper.Create(variableValue.ToString(format), attributes);
            if (textObject is null)
            {
                MBTextManager.SetTextVariable(tag, explainedTextObject);
            }
            else
            {
                textObject.SetTextVariable(tag, explainedTextObject);
            }
        }

        public enum RecursiveCaller : byte
        {
            None,
            Hero,
            Settlement,
            Clan,
            Kingdom
        }
        public enum PluralForm : byte
        {
            Singular,
            SpecificSingular,
            SpecificPlural,
            Plural
        }
    }
}