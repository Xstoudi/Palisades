
namespace Palisades
{
    public partial class App : System.Windows.Application
    {
        public App()
        {
            PalisadesManager.LoadPalisades();
            if(PalisadesManager.palisades.Count == 0)
            {
                PalisadesManager.CreatePalisade();
            }
        }
    }
}
