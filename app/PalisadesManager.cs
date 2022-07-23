using Palisades.Helpers;
using Palisades.Model;
using Palisades.View;
using Palisades.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Palisades
{
    internal static class PalisadesManager
    {
        public static readonly Dictionary<string, Palisade> palisades = new();

        public static void LoadPalisades()
        {
            string saveDirectory = PDirectory.GetPalisadesDirectory();
            PDirectory.EnsureExists(saveDirectory);

            List<PalisadeModel> loadedModels = new();

            foreach (string identifierDirname in Directory.GetDirectories(saveDirectory))
            {
                XmlSerializer deserializer = new(typeof(PalisadeModel));
                using StreamReader reader = new(Path.Combine(saveDirectory, identifierDirname, "state.xml"));
                if (deserializer.Deserialize(reader) is PalisadeModel model)
                {
                    loadedModels.Add(model);
                }
                reader.Close();
            }

            foreach(PalisadeModel loadedModel in loadedModels)
            {
                palisades.Add(loadedModel.Identifier, new Palisade(new PalisadeViewModel(loadedModel)));
            }

        }

        private static void LoadPalisade(PalisadeViewModel initialModel)
        {
            Palisade palisade = new(initialModel);
            palisades.Add(initialModel.Identifier, palisade);
        }

        public static void CreatePalisade()
        {
            PalisadeViewModel viewModel = new();
            palisades.Add(viewModel.Identifier, new Palisade(viewModel));
            viewModel.Save();
        }

        public static void DeletePalisade(string identifier)
        {
            palisades.TryGetValue(identifier, out Palisade? palisade);
            if(palisade == null)
            {
                return;
            }
            if(palisade.DataContext != null) ((PalisadeViewModel)palisade.DataContext).Delete();
            palisade.Close();
            palisades.Remove(identifier);

        }

        public static Palisade GetPalisade(string identifier)
        {
            palisades.TryGetValue(identifier, out Palisade? palisade);
            if(palisade == null)
            {
                throw new KeyNotFoundException(identifier);
            }
            return palisade;
        }
    }
}
