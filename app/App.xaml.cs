using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Palisades
{
    public partial class App : Application
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
