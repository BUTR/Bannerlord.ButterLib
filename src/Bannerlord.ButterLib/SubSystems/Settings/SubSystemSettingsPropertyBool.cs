using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings
{
    /// <inheritdoc />
    /// <summary>
    /// A switch based on a <see cref="bool"/> property.
    /// </summary>
    public record SubSystemSettingsPropertyBool<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, bool>> Property) :
        SubSystemSettingsProperty<TSubSystem, bool>(Name, Description, Property) where TSubSystem : ISubSystem;
}