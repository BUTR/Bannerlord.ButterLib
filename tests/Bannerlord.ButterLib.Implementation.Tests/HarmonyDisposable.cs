using HarmonyLib;

using System;

namespace Bannerlord.ButterLib.Implementation.Tests
{
    public sealed class HarmonyDisposable : Harmony, IDisposable
    {
        public HarmonyDisposable(string id) : base(id) { }

        public void Dispose()
        {
            UnpatchAll(Id);
        }
    }
}