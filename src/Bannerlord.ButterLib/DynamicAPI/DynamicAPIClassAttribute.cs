using System;

namespace Bannerlord.ButterLib.DynamicAPI
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DynamicAPIClassAttribute : Attribute
    {
        public string Class { get; }

        public DynamicAPIClassAttribute(string @class)
        {
            Class = @class;
        }
    }
}