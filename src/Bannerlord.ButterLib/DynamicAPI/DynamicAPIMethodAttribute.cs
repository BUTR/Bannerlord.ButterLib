using System;

namespace Bannerlord.ButterLib.DynamicAPI
{
    public class DynamicAPIMethodAttribute : Attribute
    {
        public string Method { get; }

        public DynamicAPIMethodAttribute(string method)
        {
            Method = method;
        }
    }
}