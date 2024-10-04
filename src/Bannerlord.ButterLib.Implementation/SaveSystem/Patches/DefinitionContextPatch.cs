using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;
using HarmonyLib.BUTR.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Definition;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches;

/// <summary>
/// Patches all the methods in DefinitionContext which add a new type definition from a SaveableTypeDefiner to only
/// execute if the given type is not already registered.
/// </summary>
/// <remarks>
/// <para>
/// This prevents unpredictable save [partial] failures (game reports a saving error, yet saves it anyway).
/// Further, it prevents a crash when trying to load one of such savegames.
/// </para>
/// <para>
/// In the face of mods, it's impossible for a programmer to know whether a type has already been defined elsewhere
/// (and also ridiculous to ask any programmer to always know what types have already been defined in the base game).
/// These patches fully resolve such issues.
/// </para>
/// </remarks>
internal sealed class DefinitionContextPatch
{
    private static ILogger _log = default!;

    internal static bool Enable(Harmony harmony)
    {
        var provider = ButterLibSubModule.Instance?.GetServiceProvider() ?? ButterLibSubModule.Instance?.GetTempServiceProvider();
        _log = provider?.GetService<ILogger<DefinitionContextPatch>>() ?? NullLogger<DefinitionContextPatch>.Instance;

        return Patches.Select(p => p.IsReady).All(ready => ready) && Patches.All(p => p.EnablePatch(harmony));
    }

    internal static bool Disable(Harmony harmony)
    {
        _ = harmony;
        return false;
    }

    // PATCH DEFINITIONS

    private delegate bool IsContainerDelegate(Type type);

    private static readonly IsContainerDelegate? IsContainer =
        AccessTools2.GetDelegate<IsContainerDelegate>("TaleWorlds.SaveSystem.TypeExtensions:IsContainer", [typeof(Type)]);

    private static MethodInfo? TargetTypeMethod(string name) => AccessTools2.Method($"TaleWorlds.SaveSystem.Definition.DefinitionContext:{name}");

    private static readonly Patch[] Patches =
    [
        new PrefixPatch(nameof(AddBasicTypeDefinitionPrefix),     TargetTypeMethod("AddBasicTypeDefinition")),
        new PrefixPatch(nameof(AddClassDefinitionPrefix),         TargetTypeMethod("AddClassDefinition")),
        new PrefixPatch(nameof(AddContainerDefinitionPrefix),     TargetTypeMethod("AddContainerDefinition")),
        new PrefixPatch(nameof(AddEnumDefinitionPrefix),          TargetTypeMethod("AddEnumDefinition")),
        new PrefixPatch(nameof(AddGenericClassDefinitionPrefix),  TargetTypeMethod("AddGenericClassDefinition")),
        new PrefixPatch(nameof(AddGenericStructDefinitionPrefix), TargetTypeMethod("AddGenericStructDefinition")),
        new PrefixPatch(nameof(AddInterfaceDefinitionPrefix),     TargetTypeMethod("AddInterfaceDefinition")),
        new PrefixPatch(nameof(AddRootClassDefinitionPrefix),     TargetTypeMethod("AddRootClassDefinition")),
        new PrefixPatch(nameof(AddStructDefinitionPrefix),        TargetTypeMethod("AddStructDefinition")),
        new ConstructContainerDefinitionPrefixPatch()
    ];

    // PATCH METHODS

    private static bool CanAddTypeDefinition(TypeDefinitionBase? typeDef, Dictionary<Type, TypeDefinitionBase> typeDict)
    {
        if (typeDef?.Type is null)
            return false;

        if (typeDict.ContainsKey(typeDef.Type))
        {
            _log.LogTrace("Suppressed duplicate definition of serializable type: {Type}", typeDef.Type.FullName);
            return false;
        }

        return true;
    }

    private static bool AddRootClassDefinitionPrefix(TypeDefinitionBase? rootClassDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(rootClassDefinition, ____allTypeDefinitions);

    private static bool AddClassDefinitionPrefix(TypeDefinitionBase? classDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(classDefinition, ____allTypeDefinitions);

    private static bool AddStructDefinitionPrefix(TypeDefinitionBase? structDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(structDefinition, ____allTypeDefinitions);

    private static bool AddInterfaceDefinitionPrefix(TypeDefinitionBase? interfaceDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(interfaceDefinition, ____allTypeDefinitions);

    private static bool AddEnumDefinitionPrefix(TypeDefinitionBase? enumDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(enumDefinition, ____allTypeDefinitions);

    private static bool AddContainerDefinitionPrefix(TypeDefinitionBase? containerDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(containerDefinition, ____allTypeDefinitions);

    private static bool AddBasicTypeDefinitionPrefix(TypeDefinitionBase? basicTypeDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(basicTypeDefinition, ____allTypeDefinitions);

    private static bool AddGenericClassDefinitionPrefix(TypeDefinitionBase? genericClassDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(genericClassDefinition, ____allTypeDefinitions);

    private static bool AddGenericStructDefinitionPrefix(TypeDefinitionBase? genericStructDefinition,
        Dictionary<Type, TypeDefinitionBase> ____allTypeDefinitions) =>
        CanAddTypeDefinition(genericStructDefinition, ____allTypeDefinitions);

    /// <summary>
    /// Prevent <c>DefinitionContext.ConstructContainerDefinition</c> from executing if the given
    /// <see cref="Type"/> is not a supported container type.
    /// </summary>
    /// <remarks>
    /// The target method fails to check all used values of the <see cref="ContainerType"/> enum;
    /// specifically, it does not check for the value <c>None</c>. Thus, it can easily add alleged
    /// container type definitions to the <c>DefinitionContext</c> whose fields are, besides the
    /// <see cref="Type"/>, entirely null. If this happens, the game will later crash.
    /// </remarks>
    private static bool ConstructContainerDefinitionPrefix(Type? type) => type is not null && IsContainer!(type);

    // INSTRUMENTATION

    private static bool NotNull<T>(T obj, string name, string? errPrefix = null) where T : class?
    {
        if (obj is null)
        {
            var prefix = errPrefix ?? string.Empty;
            _log.LogError("{Prefix}{Name} is null!", prefix, name);
            return false;
        }

        return true;
    }

    private abstract class Patch
    {
        private readonly string PatchMethodName;
        internal readonly MethodInfo? PatchMethod;
        internal readonly MethodInfo? TargetMethod;

        protected Patch(string patchMethodName, MethodInfo? targetMethod)
        {
            PatchMethodName = patchMethodName;
            PatchMethod = ResolvePatchMethod();
            TargetMethod = targetMethod;
        }

        internal abstract bool EnablePatch(Harmony harmony);

        internal virtual bool IsReady => ThisNotNull(PatchMethod, nameof(PatchMethod)) & ThisNotNull(TargetMethod, nameof(TargetMethod));

        private MethodInfo? ResolvePatchMethod() => AccessTools2.Method(GetType().DeclaringType!, PatchMethodName);

        protected bool ThisNotNull(object? obj, string objName) => NotNull(obj, objName, $"Patch {PatchMethodName}: ");
    }

    private sealed class ConstructContainerDefinitionPrefixPatch : PrefixPatch
    {
        internal ConstructContainerDefinitionPrefixPatch()
            : base(nameof(ConstructContainerDefinitionPrefix), TargetTypeMethod("ConstructContainerDefinition")) { }

        internal override bool IsReady => base.IsReady & ThisNotNull(IsContainer, $"{nameof(IsContainer)} delegate");
    }

    private class PrefixPatch : Patch
    {
        internal PrefixPatch(string patchMethodName, MethodInfo? targetMethod) : base(patchMethodName, targetMethod) { }

        internal override bool EnablePatch(Harmony harmony) => harmony.Patch(TargetMethod, prefix: new HarmonyMethod(PatchMethod)) is not null;
    }
}