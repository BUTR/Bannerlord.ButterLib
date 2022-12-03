using System;

namespace Bannerlord.ButterLib.DynamicAPI
{
    /// <summary>
    /// Requires a public empty constructor for instance methods
    /// </summary>
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