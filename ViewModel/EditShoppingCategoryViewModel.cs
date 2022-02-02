using System;
using System.Windows.Input;
using Organizer.View.Windows;
using Organizer.Model;
using System.Data.SqlClient;
using System.Globalization;

namespace Organizer.ViewModel
{
    class EditShoppingCategoryViewModel
    {
        private ICommand cancel;
        private ICommand clear;
        private ICommand edit;
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
        public ICommand Edit
        {
            get
            {
                edit = new EditClass();
                return edit;
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
                EditShoppingCategoryWindow editShoppingCategoryWindow = (EditShoppingCategoryWindow)parameter;
                editShoppingCategoryWindow.Close();
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
                EditShoppingCategoryWindow editShoppingCategoryWindow = (EditShoppingCategoryWindow)parameter;
                editShoppingCategoryWindow.NameInput.Text = string.Empty;
                editShoppingCategoryWindow.LimitInput.Text = string.Empty;
            }
        }
        class EditClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                EditShoppingCategoryWindow editShoppingCategoryWindow = (EditShoppingCategoryWindow)parameter;
                if(editShoppingCategoryWindow.NameInput.Text != string.Empty&&
                editShoppingCategoryWindow.LimitInput.Text != string.Empty)
                {
                    Category category = new Category(
                        Convert.ToInt32(editShoppingCategoryWindow.Title),
                        editShoppingCategoryWindow.NameInput.Text,
                        Convert.ToDecimal(editShoppingCategoryWindow.LimitInput.Text),
                        "PLN"
                        );
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        NumberFormatInfo nfi = new NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";
                        string oString = "Update Categories Set Name = '" + category.Name + "', Limit = '" + category.Limit.Value.ToString(nfi) + "' Where ID = " + category.ID;
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    editShoppingCategoryWindow.Close();
                }
            }
        }
    }
}
