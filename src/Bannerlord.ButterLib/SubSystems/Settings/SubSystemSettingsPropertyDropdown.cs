using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings
{
    /// <inheritdoc />
    /// <summary>
    /// A switch based on a <see cref="IList{string}"/> property.
    /// </summary>
    public record SubSystemSettingsPropertyDropdown<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, IList<string>>> Property, int SelectedIndex) :
        SubSystemSettingsProperty<TSubSystem, IList<string>>(Name, Description, Property) where TSubSystem : ISubSystem;
}