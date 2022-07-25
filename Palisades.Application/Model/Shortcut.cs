using Palisades.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml.Serialization;

namespace Palisades.Model
{
    [XmlInclude(typeof(LnkShortcut))]
    [XmlInclude(typeof(UrlShortcut))]
    public abstract class Shortcut
    {
        private string name;
        private string iconPath;
        private string uriOrFileAction;

        public Shortcut() : this("", "", "")
        {

        }
        public Shortcut(string name, string iconPath, string uriOrFileAction)
        {
            this.name = name;
            this.iconPath = iconPath;
            this.uriOrFileAction = uriOrFileAction;
        }

        public string Name { get { return name; } set { name = value; } }

        public string IconPath { get { return iconPath; } set { iconPath = value; } }
        public string UriOrFileAction { get { return uriOrFileAction; } set { uriOrFileAction = value; } }

        /// FIXME: Would be cool to move it out the model.

        public static string GetName(string filename)
        {
            return Path.GetFileNameWithoutExtension(filename);
        }

        public static string GetIcon(string filename, string palisadeIdentifier)
        {
            using Bitmap icon = IconExtractor.GetFileImageFromPath(filename, Helpers.Native.IconSizeEnum.LargeIcon48);

            string iconDir = PDirectory.GetPalisadeIconsDirectory(palisadeIdentifier);
            PDirectory.EnsureExists(iconDir);

            string iconFilename = Guid.NewGuid().ToString() + ".png";
            string iconPath = Path.Combine(iconDir, iconFilename);
            using FileStream fileStream = new(iconPath, FileMode.Create);
            icon.Save(fileStream, ImageFormat.Png);

            return iconPath;
        }
    }
}
