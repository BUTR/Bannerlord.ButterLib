using HarmonyLib;

using System;

namespace Bannerlord.ButterLib.Tests
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