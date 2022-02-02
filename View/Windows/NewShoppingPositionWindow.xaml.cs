using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for NewShoppingPositionWindow.xaml
    /// </summary>
    public partial class NewShoppingPositionWindow : Window
    {
        public NewShoppingPositionWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.NewShoppingPositionViewModel();
        }
    }
}
