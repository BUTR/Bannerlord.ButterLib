using System;

namespace Bannerlord.ButterLib.DynamicAPI
{
    public class DynamicAPIClassAttribute : Attribute
    {
        public string Class { get; }

        public DynamicAPIClassAttribute(string @class)
        {
            Class = @class;
        }
    }
}