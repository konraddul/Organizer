using System;
using System.Windows.Input;
using Organizer.View.Windows;
using Organizer.Model;
using System.Data.SqlClient;
using System.Globalization;

namespace Organizer.ViewModel
{
    class NewShoppingCategoryViewModel
    {
        private ICommand cancel;
        private ICommand clear;
        private ICommand add;
        public ICommand Cancel
        {
            get
            {
                cancel = new CancelClass();
                return cancel;
            }
        }
        public ICommand Clear
        {
            get
            {
                clear = new ClearClass();
                return clear;
            }
        }
        public ICommand Add
        {
            get
            {
                add = new AddClass();
                return add;
            }

        }

        class CancelClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                NewShoppingCategoryWindow newShoppingCategoryWindow = (NewShoppingCategoryWindow)parameter;
                newShoppingCategoryWindow.Close();
            }
        }
        class ClearClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                NewShoppingCategoryWindow newShoppingCategoryWindow = (NewShoppingCategoryWindow)parameter;
                newShoppingCategoryWindow.NameInput.Text = string.Empty;
                newShoppingCategoryWindow.LimitInput.Text = string.Empty;
            }
        }
        class AddClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                NewShoppingCategoryWindow newShoppingCategoryWindow = (NewShoppingCategoryWindow)parameter;
                if(newShoppingCategoryWindow.NameInput.Text != string.Empty &&
                newShoppingCategoryWindow.LimitInput.Text != string.Empty)
                {
                    Category category = new Category(
                        0,
                        newShoppingCategoryWindow.NameInput.Text,
                        Convert.ToDecimal(newShoppingCategoryWindow.LimitInput.Text),
                        "PLN"
                        );
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        NumberFormatInfo nfi = new NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";
                        string oString = "insert into Categories (Name, Limit, Currency) values ('" + category.Name + "', '" + category.Limit.Value.ToString(nfi) + "', '" + category.Limit.Symbol + "' )";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    newShoppingCategoryWindow.Close();
                }
            }
        }
    }

}
