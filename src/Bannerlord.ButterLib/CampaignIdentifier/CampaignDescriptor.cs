using Bannerlord.ButterLib.Common.Extensions;

using Microsoft.Extensions.DependencyInjection;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    /// <summary>
    /// Abstract class, serving as an interface for the CampaignDescriptor Service (implementation).
    /// </summary>
    public abstract class CampaignDescriptor
    {
        private static ICampaignDescriptorStatic? _staticInstance;
        internal static ICampaignDescriptorStatic? StaticInstance =>
            _staticInstance ??= ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ICampaignDescriptorStatic>();

        /// <summary>
        /// Initializes and returns a new instance of the class <see cref="CampaignDescriptor" />
        /// as per actual implementation.
        /// </summary>
        /// <param name="hero">The hero to be used as a descriptor base.</param>
        /// <returns>Newly initialized <see cref="CampaignDescriptor"/> instance.</returns>
        public static CampaignDescriptor? Create(Hero hero) => StaticInstance?.Create(hero);

        /// <summary>Initializes a new instance of the class <see cref="CampaignDescriptor"/>.</summary>
        protected CampaignDescriptor() { }

        /// <summary>Alphanumeric campaign ID.</summary>
        /// <value>A string, containing campaign ID.</value>
        public abstract string KeyValue { get; }

        /// <summary>A string key that is used to compare unidentified ongoing campaigns.</summary>
        /// <value>
        /// A string that contains information about gender, birthplace and culture
        /// of the <see cref="Hero"/>, that was used to initialize a descriptor.
        /// </value>
        public abstract string ImmutableKey { get; }

        /// <summary>
        /// Name of the <see cref="T:TaleWorlds.CampaignSystem.Hero" />
        /// that was used when creating this <see cref="T:Bannerlord.ButterLib.CampaignIdentifier.CampaignDescriptor"/> instance.
        /// </summary>
        /// <value>
        /// A string that contains a <see cref="F:TaleWorlds.CampaignSystem.Hero.Name"/>
        /// and a <see cref="P:TaleWorlds.CampaignSystem.Clan.Name"/> of the
        /// <see cref="Hero"/>, that was used to initialize a descriptor.
        /// </value>
        public abstract string FullCharacterName { get; }

        /// <summary>
        /// Localizable description of the campaign, based on the <see cref="Hero"/>
        /// that was used when creating this <see cref="CampaignDescriptor"/> instance.
        /// </summary>
        /// <value>A string with localized <see cref="CampaignDescriptor"/> information.</value>
        public abstract string Descriptor { get; }

        /// <summary>
        /// The <see cref="T:TaleWorlds.Core.CharacterCode"/> of the <see cref="Hero"/>
        /// that current <see cref="CampaignDescriptor"/> instance is based on.
        /// </summary>
        /// <value>
        /// A <see cref="T:TaleWorlds.Core.CharacterCode"/> for the <see cref="Hero"/>,
        /// that was used to initialize a descriptor.
        /// </value>
        public abstract CharacterCode CharacterCode { get; }
    }
}