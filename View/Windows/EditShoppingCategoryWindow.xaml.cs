using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for EditShoppingCategoryWindow.xaml
    /// </summary>
    public partial class EditShoppingCategoryWindow : Window
    {
        public EditShoppingCategoryWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.EditShoppingCategoryViewModel();
        }
    }
}
