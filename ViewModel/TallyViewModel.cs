using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Organizer.Model;
using Organizer.Types;
using Organizer.View.UserControls;

namespace Organizer.ViewModel
{
    class TallyViewModel
    {
        private string selectedMonth;
        private int selectedYear;
        private string posSum;
        private string amountSum;
        private string secondListAmountSum;
        private string secondListOverlimited;
        private List<string> monthSource;
        private List<int> yearSource;
        private List<CategoryView> categoryList;
        private List<Position> productList;
        private ICommand listIDSort;
        private ICommand listDateSort;
        private ICommand listShopSort;
        private ICommand listProductSort;
        private ICommand listCurrencySort;
        private ICommand listCategorySort;
        private ICommand secondListCategorySort;
        private ICommand secondListAmountSort;
        private ICommand secondListOverlimitSort;
        private ICommand applyFilters;
        private ICommand resetFilters;
        public string SelectedMonth
        {
            get
            {
                selectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[DateTime.Now.AddMonths(-1).Month];
                return selectedMonth;
            }
            set
            {
                selectedMonth = value;
            }
        }
        public int SelectedYear
        {
            get
            {
                selectedYear = DateTime.Now.Year;
                return selectedYear;
            }
            set
            {
                selectedYear = value;
            }
        }
        public string PosSum
        {
            get
            {
                posSum = GetPositionMth().Count.ToString();
                return posSum;
            }
        }
        public string AmountSum
        {
            get
            {
                decimal amSum = 0m;
                foreach(Position pos in GetPositionMth())
                {
                    amSum += pos.Amount.Value;
                }
                Currency cy = new Currency(amSum, "PLN");
                amountSum = cy.ToString();
                return amountSum;
            }
        }
        public string SecondListAmountSum
        {
            get
            {
                decimal amountSum = 0m;
                foreach (CategoryView catV in GetCategoriesViewMth())
                {
                    amountSum += catV.Amount.Value;
                }
                Currency amSumCy = new Currency(amountSum, "PLN");
                secondListAmountSum = amSumCy.ToString();
                return secondListAmountSum;
            }
        }
        public string SecondListOverlimited
        {
            get
            {
                decimal amountSum = 0m;
                foreach (CategoryView catV in GetCategoriesViewMth())
                {
                    amountSum += catV.Overlimit.Value;
                }
                Currency amSumCy = new Currency(amountSum, "PLN");
                secondListOverlimited = amSumCy.ToString();
                return secondListOverlimited;
            }
        }
        public List<string> MonthSource
        {
            get
            {
                monthSource = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();
                return monthSource;
            }
            set
            {
                monthSource = value;
            }
        }
        public List<int> YearSource
        {
            get
            {
                yearSource = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).ToList();
                return yearSource;
            }
            set
            {
                yearSource = value;
            }
        }
        public List<Position> ProductList
        {
            get
            {
                productList = GetPositionMth();
                return productList;
            }
        }
        public List<CategoryView> CategoryList
        {
            get
            {
                categoryList = GetCategoriesViewMth();
                return categoryList;
            }
        }
        public ICommand ListIDSort
        {
            get
            {
                listIDSort = new ListIDSortClass();
                return listIDSort;
            }
        }
        public ICommand ListDateSort
        {
            get
            {
                listDateSort = new ListDateSortClass();
                return listDateSort;
            }
        }
        public ICommand ListShopSort
        {
            get
            {
                listShopSort = new ListShopSortClass();
                return listShopSort;
            }
        }
        public ICommand ListProductSort
        {
            get
            {
                listProductSort = new ListProductSortClass();
                return listProductSort;
            }
        }
        public ICommand ListCurrencySort
        {
            get
            {
                listCurrencySort = new ListCurrencySortClass();
                return listCurrencySort;
            }
        }
        public ICommand ListCategorySort
        {
            get
            {
                listCategorySort = new ListCategorySortClass();
                return listCategorySort;
            }
        }
        public ICommand SecondListCategorySort
        {
            get
            {
                secondListCategorySort = new SecondListCategorySortClass();
                return secondListCategorySort;
            }
        }
        public ICommand SecondListAmountSort
        {
            get
            {
                secondListAmountSort = new SecondListAmountSortClass();
                return secondListAmountSort;
            }
        }
        public ICommand SecondListOverlimitSort
        {
            get
            {
                secondListOverlimitSort = new SecondListOverlimitSortClass();
                return secondListOverlimitSort;
            }
        }
        public ICommand ApplyFilters
        {
            get
            {
                applyFilters = new ApplyFiltersClass();
                return applyFilters;
            }
        }
        public ICommand ResetFilters
        {
            get
            {
                resetFilters = new ResetFiltersClass();
                return resetFilters;
            }
        }
        List<CategoryView> GetCategoriesViewMth()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string oString = "Select * from Categories";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        Category category = new Category(
                            Convert.ToInt32(oReader["ID"].ToString().Trim()),
                            oReader["Name"].ToString().Trim(),
                            Convert.ToDecimal(oReader["Limit"].ToString().Trim()),
                            oReader["Currency"].ToString().Trim()
                            );
                        categories.Add(category);
                    }
                }
                myConnection.Close();
            }
            int month = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
            List<CategoryView> categoryViewList = new List<CategoryView>();
            foreach(Category category in categories)
            {
                CategoryView catV = new CategoryView(category, GetPositionMth(), month, selectedYear);
                categoryViewList.Add(catV);
            }
            return categoryViewList;
        }
        List<Position> GetPositionMth()
        {
            List<Position> positionList = new List<Position>();
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string query = "Select * From Positions";
                SqlCommand command = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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
                        int month = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                        if(position.Date.Month == month && position.Date.Year == selectedYear)
                            positionList.Add(position);
                    }
                    myConnection.Close();
                }
            }
            return positionList;
        }

        class ListIDSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.ID).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class ListDateSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Date).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class ListShopSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Shop).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class ListProductSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Product).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class ListCurrencySortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Amount).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class ListCategorySortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<Position> positionList = (List<Position>)tallyView.ListBox.ItemsSource;
                positionList = positionList.OrderBy(u => u.Category).ToList();
                tallyView.ListBox.ItemsSource = positionList;
            }
        }
        class SecondListCategorySortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<CategoryView> categoryViewList = (List<CategoryView>)tallyView.CTListBox.ItemsSource;
                categoryViewList = categoryViewList.OrderBy(u => u.Name).ToList();
                tallyView.CTListBox.ItemsSource = categoryViewList;
            }
        }
        class SecondListAmountSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<CategoryView> categoryViewList = (List<CategoryView>)tallyView.CTListBox.ItemsSource;
                categoryViewList = categoryViewList.OrderBy(u => u.Amount).ToList();
                tallyView.CTListBox.ItemsSource = categoryViewList;
            }
        }
        class SecondListOverlimitSortClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                TallyView tallyView = (TallyView)parameter;
                List<CategoryView> categoryViewList = (List<CategoryView>)tallyView.CTListBox.ItemsSource;
                categoryViewList = categoryViewList.OrderBy(u => u.Overlimit).ToList();
                tallyView.CTListBox.ItemsSource = categoryViewList;
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
                TallyView tallyView = (TallyView)parameter;
                int month = DateTime.ParseExact(tallyView.MonthInput.SelectedItem.ToString(), "MMMM", CultureInfo.CurrentCulture).Month;
                int year = Convert.ToInt32(tallyView.YearInput.SelectedItem.ToString());
                List<Position> listBox = GetPosition(); 
                listBox = listBox.Where(u => u.Date.Month == month && u.Date.Year == year).ToList();
                tallyView.ListBox.ItemsSource = listBox;
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
                        while (reader.Read())
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
                throw new NotImplementedException();
            }
        }

        public class CategoryView
        {
            public string Name { get; set; }
            public Currency Amount { get; set; }
            public Currency Limit { get; set; }
            public Currency Overlimit { get; set; }
            public CategoryView(Category category, List<Position> positions, int month, int year)
            {
                decimal amount = 0m;
                Currency amountCy = null;
                this.Name = category.Name;
                List<Position> categoryPositions = positions.Where(u => u.Category.Name == category.Name && u.Date.Month == month && u.Date.Year == year).ToList();
                foreach(Position position in categoryPositions)
                {
                    amount += position.Amount.Value;
                }
                amountCy = new Currency(amount, "PLN");
                this.Amount = amountCy;
                if(category.Limit.Value > amountCy.Value)
                {
                    this.Overlimit = new Currency(0, "PLN");
                }
                else
                {
                    this.Overlimit = new Currency((amountCy.Value - category.Limit.Value), "PLN");
                }
                this.Limit = category.Limit;
            }
        }
    }
}
