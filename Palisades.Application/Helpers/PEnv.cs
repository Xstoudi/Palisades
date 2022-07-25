namespace Palisades.Helpers
{
    internal class PEnv
    {
        internal static bool IsDev()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
