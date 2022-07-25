using System.IO;
using System.Linq;

namespace Palisades.Model
{
    public class UrlShortcut : Shortcut
    {

        public UrlShortcut() : base()
        {
        }
        public UrlShortcut(string name, string iconPath, string uriOrFileAction) : base(name, iconPath, uriOrFileAction)
        {
        }

        public static UrlShortcut? BuildFrom(string shortcut, string palisadeIdentifier)
        {
            string? line = File.ReadLines(shortcut).FirstOrDefault((value) => value.StartsWith("URL="));
            if (line == null)
            {
                return null;
            }

            string url = line.Replace("URL=", "");
            url = url.Replace("\"", "");
            url = url.Replace("BASE", "");

            string name = Shortcut.GetName(shortcut);
            string iconPath = Shortcut.GetIcon(shortcut, palisadeIdentifier);

            return new UrlShortcut(name, iconPath, url);
        }
    }
}
