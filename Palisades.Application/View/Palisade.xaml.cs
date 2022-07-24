using System.Windows;
using Palisades.ViewModel;

namespace Palisades.View
{
    public partial class Palisade : Window
    {
        private readonly PalisadeViewModel viewModel;
        public Palisade(PalisadeViewModel defaultModel)
        {
            InitializeComponent();
            DataContext = defaultModel;
            viewModel = defaultModel;
            Show();
        }

        private void Header_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

    }
}
