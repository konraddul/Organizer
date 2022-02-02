using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for MainAppWindow.xaml
    /// </summary>
    public partial class MainAppWindow : Window
    {
        public MainAppWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.MainAppViewModel();
            Content.DataContext = null;
        }
    }
}
