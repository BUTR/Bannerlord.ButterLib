using System.Collections.Generic;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <summary>
/// An interface for foreign entities like MCM to get settings of the <typeparamref name="TSubSystem"/>.
/// </summary>
/// <typeparam name="TSubSystem">The <see cref="ISubSystem"/> that exposes the settings.</typeparam>
public interface ISubSystemSettings<TSubSystem> where TSubSystem : ISubSystem
{
    /// <summary>
    /// List of the declarations of the settings. Currently supported are:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Type</term>
    ///     <description>Class</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Switch</term>
    ///     <description><see cref="SubSystemSettingsPropertyBool{TSubSystem}"/></description>
    ///   </item>
    ///   <item>
    ///     <term>Text</term>
    ///     <description><see cref="SubSystemSettingsPropertyText{TSubSystem}"/></description>
    ///   </item>
    ///   <item>
    ///     <term>Integer</term>
    ///     <description><see cref="SubSystemSettingsPropertyInt{TSubSystem}"/></description>
    ///   </item>
    ///   <item>
    ///     <term>Floating</term>
    ///     <description><see cref="SubSystemSettingsPropertyFloat{TSubSystem}"/></description>
    ///   </item>
    ///   <item>
    ///     <term>Dropdown</term>
    ///     <description><see cref="SubSystemSettingsPropertyDropdown{TSubSystem}"/></description>
    ///   </item>
    /// </list>
    /// </summary>
    IReadOnlyCollection<SubSystemSettingsDeclaration<TSubSystem>> Declarations { get; }
}