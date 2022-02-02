using System.Windows.Controls;

namespace Organizer.View.UserControls
{
    /// <summary>
    /// Interaction logic for PositionsView.xaml
    /// </summary>
    public partial class PositionsView : UserControl
    {
        public PositionsView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.PositionsViewModel();
        }
    }
}
