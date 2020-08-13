namespace Bannerlord.ButterLib
{
    internal static class DI
    {
        public static bool TryGetImplementation<TInterface>(out TInterface implementation)
        {
            implementation = default!;
            return false;
        }

        public static bool TryCreate<TInterface>(object[] args, out TInterface implementation)
        {
            implementation = default!;
            return false;
        }
    }
}