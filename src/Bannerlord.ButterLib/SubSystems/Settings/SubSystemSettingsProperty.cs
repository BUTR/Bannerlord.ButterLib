using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings
{
    /// <inheritdoc />
    /// <summary>
    /// A property based declaration.
    /// </summary>
    /// <param name="Property">And expression that references the property.</param>
    /// <typeparam name="TProperty">Type of the property.</typeparam>
    public abstract record SubSystemSettingsProperty<TSubSystem, TProperty>(string Name, string Description, Expression<Func<TSubSystem, TProperty>> Property) :
        SubSystemSettingsDeclaration<TSubSystem>(Name, Description) where TSubSystem : ISubSystem;
}