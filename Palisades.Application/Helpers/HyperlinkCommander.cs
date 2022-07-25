using System.Windows.Documents;

namespace Palisades.Helpers
{
    internal class HyperlinkCommander : Hyperlink
    {
        protected override void OnClick()
        {
            Command.Execute(CommandParameter);
        }
    }
}
