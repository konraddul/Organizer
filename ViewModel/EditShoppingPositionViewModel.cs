using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;
using Organizer.Model;
using Organizer.View.Windows;

namespace Organizer.ViewModel
{
    class EditShoppingPositionViewModel
    {
        private ICommand cancel;
        private ICommand clear;
        private ICommand edit;
        private List<string> currencyList;
        private List<Category> categoryList;
        public List<string> CurrencyList
        {
            get
            {
                List<string> currency = new List<string>()
                {
                    "PLN",
                    "EUR"
                };
                currencyList = currency;
                return currencyList;
            }
        }
        public List<Category> CategoryList
        {
            get
            {
                categoryList = GetCategories();
                return categoryList;
            }
        }
        public ICommand Cancel
        {
            get
            {
                cancel = new CancelClass();
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
        public ICommand Clear
        {
            get
            {
                clear = new ClearClass();
                return clear;
            }
            set
            {
                clear = value;
            }
        }
        public ICommand Edit
        {
            get
            {
                edit = new EditClass();
                return edit;
            }
            set
            {
                edit = value;
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
                EditShoppingPositionWindow editShoppingPositionWindow = (EditShoppingPositionWindow)parameter;
                editShoppingPositionWindow.Close();
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
                EditShoppingPositionWindow editShoppingPositionWindow = (EditShoppingPositionWindow)parameter;
                editShoppingPositionWindow.DatePickerInput.SelectedDate = null;
                editShoppingPositionWindow.ShopInput.Text = string.Empty;
                editShoppingPositionWindow.ProductInput.Text = string.Empty;
                editShoppingPositionWindow.AmountInput.Text = string.Empty;
                editShoppingPositionWindow.CurrencyComboInput.SelectedItem = "PLN";
                editShoppingPositionWindow.CategoryComboInput.SelectedItem = null;
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
                EditShoppingPositionWindow editShoppingPositionWindow = (EditShoppingPositionWindow)parameter;
                if (
                    editShoppingPositionWindow.DatePickerInput.SelectedDate != null &&
                    editShoppingPositionWindow.ShopInput.Text != string.Empty &&
                    editShoppingPositionWindow.ProductInput.Text != string.Empty &&
                    editShoppingPositionWindow.AmountInput.Text != string.Empty &&
                    editShoppingPositionWindow.CurrencyComboInput.SelectedItem != null &&
                    editShoppingPositionWindow.CategoryComboInput.SelectedItem != null
                    )
                {
                    string loginPath = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\LoginUser.xml");
                    XDocument login = XDocument.Load(loginPath);
                    var xLogin = login.Root;
                    Position position = new Position(
                        Convert.ToInt32(editShoppingPositionWindow.Title),
                        (DateTime)editShoppingPositionWindow.DatePickerInput.SelectedDate.Value,
                        editShoppingPositionWindow.ShopInput.Text,
                        editShoppingPositionWindow.ProductInput.Text,
                        Convert.ToDecimal(editShoppingPositionWindow.AmountInput.Text),
                        editShoppingPositionWindow.CurrencyComboInput.SelectedItem.ToString(),
                        editShoppingPositionWindow.CategoryComboInput.SelectedItem.ToString(),
                        xLogin.Element("Login").Value
                        );
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        NumberFormatInfo nfi = new NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";
                        string oString = "Update Positions Set Date = '" + position.Date.ToString("yyyy-MM-dd") + "', Shop = '" + position.Shop + "', Product = '" + position.Product + "', Ammount = '" + position.Amount.Value.ToString(nfi) + "', Currency = '" + position.Amount.Symbol + "', Category = '" + position.Category.ToString() + "', Operator = '" + position.Operator.Login + "' Where ID = " + position.ID;
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    editShoppingPositionWindow.Close();
                }
                else
                    throw new Exception("Uzupełnij dane");
            }
        }
        List<Category> GetCategories()
        {
            List<Category> categoryList = new List<Category>();
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string query = "Select * From Categories";
                SqlCommand command = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Category cat = new Category(
                            (int)reader["ID"],
                            reader["Name"].ToString().Trim(),
                            (decimal)reader["Limit"],
                            reader["Currency"].ToString().Trim()
                            );
                        categoryList.Add(cat);
                    }
                    myConnection.Close();
                }
            }
            return categoryList;
        }
    }
}
