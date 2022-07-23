using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palisades.Helpers
{
    internal static class PDirectory
    {
        public static string GetAppDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), PEnv.IsDev() ? "PalisadesDev" : "Palisades");
        }

        public static string GetPalisadesDirectory()
        {
            return Path.Combine(GetAppDirectory(), "saved");
        }

        public static string GetPalisadeDirectory(string identifier)
        {
            return Path.Combine(GetPalisadesDirectory(), identifier);
        }

        public static string GetPalisadeIconsDirectory(string identifier)
        {
            return Path.Combine(GetPalisadeDirectory(identifier), "icons");
        }

        public static void EnsureExists(string directory)
        {
            DirectoryInfo infos = new(directory);
            if(!infos.Exists)
            {
                infos.Create();
            }
        }
    }
}
