namespace Bannerlord.ButterLib.SubSystems.Settings
{
    /// <summary>
    /// The basic declaration entity.
    /// </summary>
    /// <param name="Name">The name of the settings entry.</param>
    /// <param name="Description">The description of the settings entry.</param>
    /// <typeparam name="TSubSystem">The <see cref="ISubSystem"/> that exposes the settings.</typeparam>
    public abstract record SubSystemSettingsDeclaration<TSubSystem>(string Name, string Description)  where TSubSystem : ISubSystem;
}