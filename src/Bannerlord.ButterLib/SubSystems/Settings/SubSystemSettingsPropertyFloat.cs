using System;
using System.Linq.Expressions;

namespace Bannerlord.ButterLib.SubSystems.Settings;

/// <inheritdoc />
/// <summary>
/// A numeric based on a <see cref="float"/> property.
/// </summary>
public record SubSystemSettingsPropertyFloat<TSubSystem>(string Name, string Description, Expression<Func<TSubSystem, float>> Property, float MinValue, float MaxValue) :
    SubSystemSettingsProperty<TSubSystem, float>(Name, Description, Property) where TSubSystem : ISubSystem;