using System.Windows.Controls;

namespace Organizer.View.UserControls
{
    /// <summary>
    /// Interaction logic for CategoriesView.xaml
    /// </summary>
    public partial class CategoriesView : UserControl
    {
        public CategoriesView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.CategoriesViewModel();
        }
    }
}
