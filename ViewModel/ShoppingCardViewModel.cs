using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Organizer.ViewModel
{
    public class ShoppingCardViewModel
    {
        private ICommand allPositions;
        private ICommand categories;
        private ICommand tally;

        public ICommand ShowAllPositions
        {
            get
            {
                allPositions = new AllPositionsClass();
                return allPositions;
            }
            set
            {
                allPositions = value;
            }
        }
        public ICommand ShowCategories
        {
            get
            {
                categories = new CategoriesClass();
                return categories;
            }
            set
            {
                categories = value;
            }
        }
        public ICommand ShowTally
        {
            get
            {
                tally = new TallyClass();
                return tally;
            }
            set
            {
                tally = value;
            }
        }

        private class AllPositionsClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                ContentControl cc = (ContentControl)parameter;
                cc.DataContext = new ViewModel.PositionsViewModel();
            }
        }
        private class CategoriesClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                ContentControl cc = (ContentControl)parameter;
                cc.DataContext = new ViewModel.CategoriesViewModel();
            }
        }
        private class TallyClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                ContentControl cc = (ContentControl)parameter;
                cc.DataContext = new ViewModel.TallyViewModel();
            }
        }
    }
}
