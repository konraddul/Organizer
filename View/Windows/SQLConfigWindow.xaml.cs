using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for SQLConfigWindow.xaml
    /// </summary>
    public partial class SQLConfigWindow : Window
    {
        public SQLConfigWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.SQLConfigViewModel();
        }
    }
}
