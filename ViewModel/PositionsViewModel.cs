using System;
using System.Collections.Generic;
using Organizer.Model;
using System.Data.SqlClient;
using System.Windows.Input;
using Organizer.View.UserControls;
using Organizer.View.Windows;
using System.Linq;
using Organizer.Types;

namespace Organizer.ViewModel
{
    class PositionsViewModel
    {
        private List<Position> positionList;
        private ICommand sortById;
        private ICommand sortByDate;
        private ICommand sortByShop;
        private ICommand sortByProduct;
        private ICommand sortByAmount;
        private ICommand sortByCategory;
        private ICommand applyFilters;
        private ICommand resetFilters;
        private ICommand newPosition;
        private ICommand deletePosition;
        private ICommand editPosition;
        private string posSum;
        private string amountSum;
        public List<Position> PositionList 
        {
            get
            {
                positionList = GetPosition();
                return positionList;
            }
            set
            {
                positionList = value;
            }
        }
        public ICommand SortByID 
        { 
            get
            {
                sortById = new SortByIdClass();
                return sortById;
            }
            set
            {
                sortById = value;
            }
        }
        public ICommand SortByDate
        {
            get
            {
                sortByDate = new SortByDateClass();
                return sortByDate;
            }
            set
            {
                sortByDate = value;
            }
        }
        public ICommand SortByShop
        {
            get
            {
                sortByShop = new SortByShopClass();
                return sortByShop;
            }
            set
            {
                sortByShop = value;
            }
        }
        public ICommand SortByProduct
        {
            get
            {
                sortByProduct = new SortByProductClass();
                return sortByProduct;
            }
            set
            {
                sortByProduct = value;
            }
        }
        public ICommand SortByAmount
        {
            get
            {
                sortByAmount = new SortByAmountClass();
                return sortByAmount;
            }
            set
            {
                sortByAmount = value;
            }
        }
        public ICommand SortByCategory
        {
            get
            {
                sortByCategory = new SortByCategoryClass();
                return sortByCategory;
            }
            set
            {
                sortByCategory = value;
            }
        }
        public ICommand ApplyFilters 
        {
            get
            {
                applyFilters = new ApplyFiltersClass();
                return applyFilters;
            } 
            set
            {
                applyFilters = value;
            }
        }
        public ICommand ResetFilters 
        { 
            get
            {
                resetFilters = new ResetFiltersClass();
                return resetFilters;
            }
            set
            {
                resetFilters = value;
            }
        }
        public ICommand NewPosition 
        {
            get
            {
                newPosition = new NewPositionClass();
                return newPosition;
            }
            set
            {
                newPosition = value;
            }
        }
        public ICommand DeletePosition
        {
            get
            {
                deletePosition = new DeletePositionClass();
                return deletePosition;
            }
            set
            {
                deletePosition = value;
            }
        }
        public ICommand EditPosition
        {
            get
            {
                editPosition = new EditPositionClass();
                return editPosition;
            }
            set
            {
                editPosition = value;
            }
        }
        public string PosSum 
        {
            get
            {
                int sum = 0;
                foreach(Position pos in positionList)
                {
                    sum++;
                }
                posSum = sum.ToString();
                return posSum;
            } 
        }
        public string AmountSum 
        { 
            get
            {
                decimal amount = 0m;
                foreach(Position pos in positionList)
                {
                    if(pos.Amount.Symbol == "PLN") amount += pos.Amount.Value;
                }
                Currency currency = new Currency(amount, "PLN");
                amountSum = currency.ToString();
                return amountSum;
            }
        }
        List<Position> GetPosition()
        {
            List<Position> positionList = new List<Position>();
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string query = "Select * From Positions";
                SqlCommand command = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Position position = new Position(
                            (int)reader["ID"], 
                            (DateTime)reader["Date"],
                            reader["Shop"].ToString().Trim(),
                            reader["Product"].ToString().Trim(),
                            (decimal)reader["Ammount"],
                            reader["Currency"].ToString().Trim(),
                            reader["Category"].ToString().Trim(),
                            reader["Operator"].ToString().Trim()
                            );
                        positionList.Add(position);
                    }
                    myConnection.Close();
                }
            }
            return positionList;
        }
        class SortByIdClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.ID).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class SortByDateClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Date).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class SortByShopClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Shop).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class SortByProductClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Product).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class SortByAmountClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Amount).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class SortByCategoryClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                List<Position> positionList = (List<Position>)positionsView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Category).ToList();
                positionsView.ListBox.ItemsSource = positionList;
            }
        }
        class ApplyFiltersClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                PositionsViewModel positionsViewModel = new PositionsViewModel();
                List<Position> list = positionsViewModel.GetPosition();
                List<Position> list2 = new List<Position>();
                List<Position> list3 = new List<Position>();
                string posSum = string.Empty;
                string amountSum = string.Empty;
                int pSum = 0;
                decimal aSum = 0m;
                Currency aSumCy = null;
                if (positionsView.DateFromFilter.SelectedDate != null)
                {
                    foreach(Position pos in list)
                    {
                        if (pos.Date >= positionsView.DateFromFilter.SelectedDate)
                        {
                            list2.Add(pos);
                        }
                    }
                    list = list2;
                }
                if(positionsView.DateToFilter.SelectedDate != null)
                {
                    foreach (Position pos in list)
                    {
                        if (pos.Date <= positionsView.DateToFilter.SelectedDate)
                        {
                            list3.Add(pos);
                        }
                    }
                    list = list3;
                }
                foreach(Position pos in list)
                {
                    pSum++;
                    if(pos.Amount.Symbol == "PLN") aSum += pos.Amount.Value;
                }
                posSum = pSum.ToString();
                aSumCy = new Currency(aSum, "PLN");
                positionsView.LpSum.Text = posSum;
                positionsView.AmmountSum.Text = aSumCy.ToString();
                positionsView.ListBox.ItemsSource = list;
            }
        }
        class ResetFiltersClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                PositionsViewModel positionsViewModel = new PositionsViewModel();
                string posSum = string.Empty;
                string amountSum = string.Empty;
                int pSum = 0;
                decimal aSum = 0m;
                Currency aSumCy = null;
                foreach (Position pos in positionsViewModel.GetPosition())
                {
                    pSum++;
                    if (pos.Amount.Symbol == "PLN") aSum += pos.Amount.Value;
                }
                posSum = pSum.ToString();
                aSumCy = new Currency(aSum, "PLN");
                positionsView.LpSum.Text = posSum;
                positionsView.AmmountSum.Text = aSumCy.ToString();
                positionsView.ListBox.ItemsSource = positionsViewModel.GetPosition();
                positionsView.DateFromFilter.SelectedDate = null;
                positionsView.DateToFilter.SelectedDate = null;
            }
        }
        class NewPositionClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                PositionsViewModel positionsViewModel = new PositionsViewModel();
                NewShoppingPositionWindow newShoppingPositionWindow = new NewShoppingPositionWindow();
                newShoppingPositionWindow.CurrencyComboInput.SelectedItem = "PLN";
                newShoppingPositionWindow.ShowDialog();
                positionsView.ListBox.ItemsSource = positionsViewModel.GetPosition();
                string posSum = string.Empty;
                string amountSum = string.Empty;
                int pSum = 0;
                decimal aSum = 0m;
                Currency aSumCy = null;
                foreach (Position pos in positionsViewModel.GetPosition())
                {
                    pSum++;
                    if (pos.Amount.Symbol == "PLN") aSum += pos.Amount.Value;
                }
                posSum = pSum.ToString();
                aSumCy = new Currency(aSum, "PLN");
                positionsView.LpSum.Text = posSum;
                positionsView.AmmountSum.Text = aSumCy.ToString();

            }
        }
        class DeletePositionClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                PositionsViewModel positionsViewModel = new PositionsViewModel();
                if (positionsView.ListBox.SelectedItem != null)
                {
                    Position pos = (Position)positionsView.ListBox.SelectedItem;
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        string oString = "Delete from Positions where ID = '" + pos.ID + "'";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    positionsView.ListBox.ItemsSource = positionsViewModel.GetPosition();
                    string posSum = string.Empty;
                    string amountSum = string.Empty;
                    int pSum = 0;
                    decimal aSum = 0m;
                    Currency aSumCy = null;
                    foreach (Position position in positionsViewModel.GetPosition())
                    {
                        pSum++;
                        if (position.Amount.Symbol == "PLN") aSum += position.Amount.Value;
                    }
                    posSum = pSum.ToString();
                    aSumCy = new Currency(aSum, "PLN");
                    positionsView.LpSum.Text = posSum;
                    positionsView.AmmountSum.Text = aSumCy.ToString();
                }
            }
        }
        class EditPositionClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                PositionsView positionsView = (PositionsView)parameter;
                
                PositionsViewModel positionsViewModel = new PositionsViewModel();
                if (positionsView.ListBox.SelectedItem != null)
                {
                    Position position = (Position)positionsView.ListBox.SelectedItem;
                    EditShoppingPositionWindow editShoppingPositionWindow = new EditShoppingPositionWindow();
                    editShoppingPositionWindow.Title = position.ID.ToString();
                    editShoppingPositionWindow.DatePickerInput.SelectedDate = position.Date;
                    editShoppingPositionWindow.ShopInput.Text = position.Shop;
                    editShoppingPositionWindow.ProductInput.Text = position.Product;
                    editShoppingPositionWindow.AmountInput.Text = position.Amount.Value.ToString();
                    editShoppingPositionWindow.CurrencyComboInput.SelectedItem = position.Amount.Symbol;

                    editShoppingPositionWindow.ShowDialog();
                    positionsView.ListBox.ItemsSource = positionsViewModel.GetPosition();
                    string posSum = string.Empty;
                    string amountSum = string.Empty;
                    int pSum = 0;
                    decimal aSum = 0m;
                    Currency aSumCy = null;
                    foreach (Position pos in positionsViewModel.GetPosition())
                    {
                        pSum++;
                        if (pos.Amount.Symbol == "PLN") aSum += pos.Amount.Value;
                    }
                    posSum = pSum.ToString();
                    aSumCy = new Currency(aSum, "PLN");
                    positionsView.LpSum.Text = posSum;
                    positionsView.AmmountSum.Text = aSumCy.ToString();
                }
                
            }
        }
    }
}
