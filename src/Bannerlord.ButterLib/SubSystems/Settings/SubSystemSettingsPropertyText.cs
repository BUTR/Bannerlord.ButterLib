using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <inheritdoc />
/// <summary>
/// A text based on a <see cref="string"/> property.
/// </summary>
public record SubSystemSettingsPropertyText<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, string>> Property) :
    SubSystemSettingsProperty<TSubSystem, string>(Name, Description, Property) where TSubSystem : ISubSystem;