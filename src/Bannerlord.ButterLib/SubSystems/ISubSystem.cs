namespace Bannerlord.ButterLib.SubSystems;

public interface ISubSystem
{
    string Id { get; }
    string Name { get; }
    string Description { get; }
    bool IsEnabled { get; }
    bool CanBeDisabled { get; }
    bool CanBeSwitchedAtRuntime { get; }

    void Enable();
    void Disable();
}