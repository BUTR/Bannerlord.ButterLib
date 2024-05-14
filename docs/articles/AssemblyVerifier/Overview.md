> [!IMPORTANT]
> This SubSystem is obsolete!
>

``AssemblyVerifier`` used to give the ability to pre-load an assembly and check if it's compatible with the game by calling [``Assembly.GetTypes()``](xref:System.Reflection.Assembly.GetTypes). This feature is obsolete and no longer supported.
```csharp
string dependencyPath;
string assemblyPath;

using var verifier = new AssemblyVerifier("Test");
var loader = verifier.GetLoader(out var exception);

// The AssemblyVerifier will have every loaded assembly by the game within itself.
// You can load additional assemblies if needed.
loader.LoadFile(dependencyPath);

var isCompatible = loader.LoadFileAndTest(assemblyPath);
```