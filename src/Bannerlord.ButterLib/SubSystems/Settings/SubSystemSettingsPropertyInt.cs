using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <inheritdoc />
/// <summary>
/// A numeric based on a <see cref="float"/> property.
/// </summary>
public record SubSystemSettingsPropertyInt<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, int>> Property, int MinValue, int MaxValue) :
    SubSystemSettingsProperty<TSubSystem, int>(Name, Description, Property) where TSubSystem : ISubSystem;