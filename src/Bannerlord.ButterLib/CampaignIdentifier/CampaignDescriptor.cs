using Bannerlord.ButterLib.Common.Extensions;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace Bannerlord.ButterLib.CampaignIdentifier
{
    public abstract class CampaignDescriptor
    {
        public static CampaignDescriptor? Create() => SubModule.ServiceProvider.Create<CampaignDescriptor>();


        /// <summary>Initializes a new instance of the class <see cref="CampaignDescriptor" />.</summary>
        protected CampaignDescriptor() { }

        /// <summary>Alphanumeric campaign ID.</summary>
        public abstract string KeyValue { get; }

        /// <summary>A string key that is used to compare unidentified ongoing campaigns.</summary>
        /// <remarks>Contains information about gender, birthplace and culture of the initial character.</remarks>
        public abstract string ImmutableKey { get; }

        /// <summary>
        /// Name of the <see cref="T:TaleWorlds.CampaignSystem.Hero" />
        /// that was used when creating this <see cref="T:Bannerlord.ButterLib.CampaignIdentifier.CampaignDescriptor" /> instance.
        /// </summary>
        public abstract string FullCharacterName { get; }

        /// <summary>
        /// Localizable description of the campaign, based on the <see cref="Hero" /> 
        /// that was used when creating this <see cref="CampaignDescriptor" /> instance.
        /// </summary>
        public abstract string Descriptor { get; }

        /// <summary>
        /// The <see cref="T:TaleWorlds.Core.CharacterCode" /> of the <see cref="Hero" /> 
        /// that current <see cref="CampaignDescriptor" /> instance is based on.
        /// </summary>
        public abstract CharacterCode CharacterCode { get; }
    }
}