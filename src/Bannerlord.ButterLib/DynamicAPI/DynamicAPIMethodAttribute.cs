using System;

namespace Bannerlord.ButterLib.DynamicAPI;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DynamicAPIMethodAttribute : Attribute
{
    public string Method { get; }

    public DynamicAPIMethodAttribute(string method)
    {
        Method = method;
    }
}