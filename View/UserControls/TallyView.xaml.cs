using System.Windows.Controls;

namespace Organizer.View.UserControls
{
    /// <summary>
    /// Interaction logic for TallyView.xaml
    /// </summary>
    public partial class TallyView : UserControl
    {
        public TallyView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.TallyViewModel();
        }
    }
}
