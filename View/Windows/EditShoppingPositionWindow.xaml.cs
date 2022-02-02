using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for EditShoppingPositionWindow.xaml
    /// </summary>
    public partial class EditShoppingPositionWindow : Window
    {
        public EditShoppingPositionWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.EditShoppingPositionViewModel();
        }
    }
}
