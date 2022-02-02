using System.Windows.Controls;

namespace Organizer.View.UserControls
{
    /// <summary>
    /// Interaction logic for ShoppingCardView.xaml
    /// </summary>
    public partial class ShoppingCardView : UserControl
    {
        public ShoppingCardView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.ShoppingCardViewModel();
            this.ShoppingContent.DataContext = null;
        }
    }
}
