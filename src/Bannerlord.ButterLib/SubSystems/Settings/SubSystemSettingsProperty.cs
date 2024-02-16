using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <summary>
/// A property based declaration.
/// </summary>
/// <param name="Name">The name of the settings entry.</param>
/// <param name="Description">The description of the settings entry.</param>
/// <param name="Property">And expression that references the property.</param>
/// <typeparam name="TSubSystem">The <see cref="ISubSystem"/> that exposes the settings.</typeparam>
/// <typeparam name="TProperty">Type of the property.</typeparam>
public abstract record SubSystemSettingsProperty<TSubSystem, TProperty>(string Name, string Description, Expression<Func<TSubSystem, TProperty>> Property) :
    SubSystemSettingsDeclaration<TSubSystem>(Name, Description) where TSubSystem : ISubSystem;