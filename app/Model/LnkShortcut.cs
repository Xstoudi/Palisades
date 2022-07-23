namespace Palisades.Model
{
    public class LnkShortcut : Shortcut
    {

        public LnkShortcut() : base()
        {
        }
        public LnkShortcut(string name, string iconPath, string uriOrFileAction) : base(name, iconPath, uriOrFileAction)
        {
        }

        public static LnkShortcut? BuildFrom(string shortcut, string palisadeIdentifier)
        {
            IWshRuntimeLibrary.WshShell shell = new();
            IWshRuntimeLibrary.IWshShortcut link = shell.CreateShortcut(shortcut);

            string name = Shortcut.GetName(shortcut);
            string iconPath = Shortcut.GetIcon(shortcut, palisadeIdentifier);

            return new LnkShortcut(name, iconPath, link.TargetPath);
        }
    }
}
