using System.Windows.Documents;

namespace Palisades.Helpers
{
    public class HyperlinkCommander : Hyperlink
    {
        protected override void OnClick()
        {
            Command.Execute(CommandParameter);
        }
    }
}
