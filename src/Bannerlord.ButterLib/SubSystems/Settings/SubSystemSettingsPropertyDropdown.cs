using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <inheritdoc />
/// <summary>
/// <see cref="IList{T}">IList</see>&lt;<see cref="System.String" />&gt;
/// </summary>
public record SubSystemSettingsPropertyDropdown<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, IList<string>>> Property, int SelectedIndex) :
    SubSystemSettingsProperty<TSubSystem, IList<string>>(Name, Description, Property) where TSubSystem : ISubSystem
{
    public int SelectedIndex { get; set; } = SelectedIndex;
}