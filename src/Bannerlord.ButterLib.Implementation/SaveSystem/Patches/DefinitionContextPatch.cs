using Bannerlord.ButterLib.Common.Extensions;

using HarmonyLib;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Definition;

namespace Bannerlord.ButterLib.Implementation.SaveSystem.Patches
{
    /*
     * <summary>
     * Patches all the methods in DefinitionContext which add a new type definition from a SaveableTypeDefiner to only
     * execute if the given type is not already registered.
     * </summary>
     * <remarks>
     * <para>
     * This prevents unpredictable save [partial] failures (game reports a saving error, yet saves it anyway).
     * Further, it prevents a crash when trying to load one of such savegames.
     * </para>
     * <para>
     * In the face of mods, it's impossible for a programmer to know whether a type has already been defined elsewhere
     * (and also ridiculous to ask any programmer to always know what types have already been defined in the base game).
     * These patches fully resolve such issues.
     * </para>
     * </remarks>
     */
    internal sealed class DefinitionContextPatch
    {
        private static ILogger _log = default!;

        internal static bool Apply(Harmony harmony)
        {
            _log = ButterLibSubModule.Instance?.GetServiceProvider()?.GetRequiredService<ILogger<DefinitionContextPatch>>()
                   ?? NullLogger<DefinitionContextPatch>.Instance;

            return Patches.Select(p => p.IsReady).All(ready => ready) && Patches.All(p => p.ApplyPrefix(harmony));
        }


        // PATCH DEFINITIONS

        private class NoType { }
        private static readonly Type? TargetType = typeof(MetaData).Assembly.GetType("TaleWorlds.SaveSystem.Definition.DefinitionContext");

        private static MethodInfo? TargetTypeMethod(string name) => AccessTools.Method(TargetType ?? typeof(NoType), name);

        private static readonly Patch[] Patches = new Patch[]
        {
            new("AddRootClassDefinitionPrefix",     TargetTypeMethod("AddRootClassDefinition")),
            new("AddClassDefinitionPrefix",         TargetTypeMethod("AddClassDefinition")),
            new("AddStructDefinitionPrefix",        TargetTypeMethod("AddStructDefinition")),
            new("AddInterfaceDefinitionPrefix",     TargetTypeMethod("AddInterfaceDefinition")),
            new("AddEnumDefinitionPrefix",          TargetTypeMethod("AddEnumDefinition")),
            new("AddContainerDefinitionPrefix",     TargetTypeMethod("AddContainerDefinition")),
            new("AddBasicTypeDefinitionPrefix",     TargetTypeMethod("AddBasicTypeDefinition")),
            new("AddGenericClassDefinitionPrefix",  TargetTypeMethod("AddGenericClassDefinition")),
            new("AddGenericStructDefinitionPrefix", TargetTypeMethod("AddGenericStructDefinition")),
        };

        // PATCH METHODS

        private static bool CanAddTypeDefinition(TypeDefinitionBase? typeDef, Dictionary<Type, TypeDefinitionBase> typeDict)
        {
            if (typeDef is null)
                return false;

            if (typeDict.ContainsKey(typeDef.Type))
            {
                _log.LogTrace("Suppressed duplicate SaveSystem registration of type {type}", typeDef.Type.FullName);
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

        // UTILITY

        private class Patch
        {
            internal readonly string      PatchMethodName;
            internal readonly MethodInfo? PatchMethod;
            internal readonly MethodInfo? TargetMethod;

            internal Patch(string patchMethodName, MethodInfo? targetMethod)
            {
                PatchMethodName = patchMethodName;
                PatchMethod = AccessTools.Method(typeof(DefinitionContextPatch), patchMethodName);
                TargetMethod = targetMethod;
            }

            internal bool ApplyPrefix(Harmony harmony) => harmony.Patch(TargetMethod, prefix: new HarmonyMethod(PatchMethod)) is not null;

            internal bool IsReady => MethodNotNull(PatchMethod, nameof(PatchMethod)) & MethodNotNull(TargetMethod, nameof(TargetMethod));

            private bool MethodNotNull(MethodInfo? method, string methodName) => NotNull(method, methodName, $"Patch {PatchMethodName}: ");
        }

        private static bool NotNull<T>(T obj, string name, string? errPrefix = null) where T : class?
        {
            if (obj is null)
            {
                var prefix = errPrefix is null ? string.Empty : errPrefix;
                _log.LogError($"{prefix}{name} is null!");
                return false;
            }

            return true;
        }
    }
}
