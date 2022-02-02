using System;
using System.Collections.Generic;
using Organizer.Model;
using System.Data.SqlClient;
using System.Windows.Input;
using Organizer.View.Windows;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

namespace Organizer.ViewModel
{
    class NewShoppingPositionViewModel
    {
        private List<string> currencyList;
        private List<Category> categoryList;
        private ICommand cancel;
        private ICommand clear;
        private ICommand add;
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
        public ICommand Add
        {
            get
            {
                add = new AddClass();
                return add;
            }
            set
            {
                add = value;
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
                    while(reader.Read())
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

        class CancelClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                NewShoppingPositionWindow newShoppingPositionWindow = (NewShoppingPositionWindow)parameter;
                newShoppingPositionWindow.Close();
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
                NewShoppingPositionWindow newShoppingPositionWindow = (NewShoppingPositionWindow)parameter;
                newShoppingPositionWindow.DatePickerInput.SelectedDate = null;
                newShoppingPositionWindow.ShopInput.Text = string.Empty;
                newShoppingPositionWindow.ProductInput.Text = string.Empty;
                newShoppingPositionWindow.AmountInput.Text = string.Empty;
                newShoppingPositionWindow.CurrencyComboInput.SelectedItem = "PLN";
                newShoppingPositionWindow.CategoryComboInput.SelectedItem = null;
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
                NewShoppingPositionWindow newShoppingPositionWindow = (NewShoppingPositionWindow)parameter;
                if (
                    newShoppingPositionWindow.DatePickerInput.SelectedDate != null &&
                    newShoppingPositionWindow.ShopInput.Text != string.Empty &&
                    newShoppingPositionWindow.ProductInput.Text != string.Empty &&
                    newShoppingPositionWindow.AmountInput.Text != string.Empty &&
                    newShoppingPositionWindow.CurrencyComboInput.SelectedItem != null &&
                    newShoppingPositionWindow.CategoryComboInput.SelectedItem != null
                    )
                {
                    int id = 0;
                    string loginPath = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\LoginUser.xml");
                    XDocument login = XDocument.Load(loginPath);
                    var xLogin = login.Root;
                    Position position = new Position(
                        id,
                        (DateTime)newShoppingPositionWindow.DatePickerInput.SelectedDate.Value,
                        newShoppingPositionWindow.ShopInput.Text,
                        newShoppingPositionWindow.ProductInput.Text,
                        Convert.ToDecimal(newShoppingPositionWindow.AmountInput.Text),
                        newShoppingPositionWindow.CurrencyComboInput.SelectedItem.ToString(),
                        newShoppingPositionWindow.CategoryComboInput.SelectedItem.ToString(),
                        xLogin.Element("Login").Value
                        );
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        NumberFormatInfo nfi = new NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";
                        string oString = "insert into Positions (Date, Shop, Product, Ammount, Currency, Category, Operator) values ('" + position.Date.ToString("yyyy-MM-dd") + "', '" + position.Shop + "', '" + position.Product + "', '" + position.Amount.Value.ToString(nfi) + "', '" + position.Amount.Symbol + "', '" + position.Category.ToString() + "', '" + position.Operator.Login + "' )";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    newShoppingPositionWindow.Close();
                }
                else
                throw new Exception("Uzupełnij dane");
            }
        }
    }
}
