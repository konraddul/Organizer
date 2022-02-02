using System.Windows;

namespace Organizer.View.Windows
{
    /// <summary>
    /// Interaction logic for NewShoppingCategoryView.xaml
    /// </summary>
    public partial class NewShoppingCategoryWindow : Window
    {
        public NewShoppingCategoryWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.NewShoppingCategoryViewModel();
        }
    }
}
