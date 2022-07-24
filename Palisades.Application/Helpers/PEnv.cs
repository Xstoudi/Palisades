namespace Palisades.Helpers
{
    internal class PEnv
    {
        public static bool IsDev()
        {
            #if DEBUG
            return true;
            #else
            return false;
            #endif
        }
    }
}
