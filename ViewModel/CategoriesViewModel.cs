using Organizer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Input;
using Organizer.Types;
using Organizer.View.UserControls;
using System.Linq;
using Organizer.View.Windows;

namespace Organizer.ViewModel
{
    class CategoriesViewModel
    {
        private ICommand sortById;
        private ICommand sortByName;
        private ICommand sortByLimit;
        private ICommand addCategory;
        private ICommand deleteCategory;
        private ICommand editCategory;
        private string monthLimit;
        private List<Category> categoryList;
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
        public ICommand SortByName
        {
            get
            {
                sortByName = new SortByNameClass();
                return sortByName;
            }
            set
            {
                sortByName = value;
            }
        }
        public ICommand SortByLimit
        {
            get
            {
                sortByLimit = new SortByLimitClass();
                return sortByLimit;
            }
            set
            {
                sortByLimit = value;
            }
        }
        public ICommand AddCategory
        {
            get
            {
                addCategory = new AddCategoryClass();
                return addCategory;
            }
            set
            {
                addCategory = value;
            }
        }
        public ICommand DeleteCategory
        {
            get
            {
                deleteCategory = new DeleteCategoryClass();
                return deleteCategory;
            }
            set
            {
                deleteCategory = value;
            }
        }
        public ICommand EditCategory
        {
            get
            {
                editCategory = new EditCategoryClass();
                return editCategory;
            }
            set
            {
                editCategory = value;
            }
        }
        public string MonthLimit
        {
            get
            {
                decimal catSum = 0m;
                foreach(Category cat in GetCategories())
                {
                    catSum += cat.Limit.Value;
                }
                Currency catSumCy = new Currency(catSum, "PLN");
                monthLimit = catSumCy.ToString();
                return monthLimit;
            }
            set
            {
                monthLimit = value;
            }
        }
        public List<Category> CategoryList 
        {
            get
            {
                categoryList = GetCategories();
                return categoryList;
            } 
            set
            {
                categoryList = value;
            }
        }
        List<Category> GetCategories()
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
            return categories;
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
                CategoriesView categoriesView = (CategoriesView)parameter;
                List<Category> categoryList = (List<Category>)categoriesView.ListBox.ItemsSource;
                categoryList = categoryList.OrderBy(u => u.ID).ToList();
                categoriesView.ListBox.ItemsSource = categoryList;
            }
        }
        class SortByNameClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                CategoriesView categoriesView = (CategoriesView)parameter;
                List<Category> categoryList = (List<Category>)categoriesView.ListBox.ItemsSource;
                categoryList = categoryList.OrderBy(u => u.Name).ToList();
                categoriesView.ListBox.ItemsSource = categoryList;
            }
        }
        class SortByLimitClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                CategoriesView categoriesView = (CategoriesView)parameter;
                List<Category> categoryList = (List<Category>)categoriesView.ListBox.ItemsSource;
                categoryList = categoryList.OrderBy(u => u.Limit).ToList();
                categoriesView.ListBox.ItemsSource = categoryList;
            }
        }
        class AddCategoryClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                CategoriesView categoriesView = (CategoriesView)parameter;
                CategoriesViewModel categoriesViewModel = new CategoriesViewModel();
                NewShoppingCategoryWindow newShoppingCategoryWindow = new NewShoppingCategoryWindow();
                newShoppingCategoryWindow.ShowDialog();
                categoriesView.ListBox.ItemsSource = categoriesViewModel.GetCategories();
                decimal catSum = 0m;
                foreach (Category cat in categoriesViewModel.GetCategories())
                {
                    catSum += cat.Limit.Value;
                }
                Currency catSumCy = new Currency(catSum, "PLN");
                categoriesView.LimitSum.Text = catSumCy.ToString();
            }
        }
        class DeleteCategoryClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                CategoriesView categoriesView = (CategoriesView)parameter;
                CategoriesViewModel categoriesViewModel = new CategoriesViewModel();
                if (categoriesView.ListBox.SelectedItem != null)
                {
                    Category cat = (Category)categoriesView.ListBox.SelectedItem;
                    using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
                    {
                        string oString = "Delete from Categories where ID = '" + cat.ID + "'";
                        SqlCommand oCmd = new SqlCommand(oString, myConnection);
                        myConnection.Open();
                        oCmd.ExecuteNonQuery();
                        myConnection.Close();
                    }
                    categoriesView.ListBox.ItemsSource = categoriesViewModel.GetCategories();
                    decimal catSum = 0m;
                    foreach (Category cate in categoriesViewModel.GetCategories())
                    {
                        catSum += cate.Limit.Value;
                    }
                    Currency catSumCy = new Currency(catSum, "PLN");
                    categoriesView.LimitSum.Text = catSumCy.ToString();
                }
            }
        }
        class EditCategoryClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                CategoriesView categoriesView = (CategoriesView)parameter;
                CategoriesViewModel categoriesViewModel = new CategoriesViewModel();
                if (categoriesView.ListBox.SelectedItem != null)
                {
                    Category category = (Category)categoriesView.ListBox.SelectedItem;
                    EditShoppingCategoryWindow editShoppingCategoryWindow = new EditShoppingCategoryWindow();
                    editShoppingCategoryWindow.NameInput.Text = category.Name;
                    editShoppingCategoryWindow.LimitInput.Text = category.Limit.Value.ToString();
                    editShoppingCategoryWindow.Title = category.ID.ToString();
                    editShoppingCategoryWindow.ShowDialog();
                    categoriesView.ListBox.ItemsSource = categoriesViewModel.GetCategories();
                    decimal catSum = 0m;
                    foreach (Category cate in categoriesViewModel.GetCategories())
                    {
                        catSum += cate.Limit.Value;
                    }
                    Currency catSumCy = new Currency(catSum, "PLN");
                    categoriesView.LimitSum.Text = catSumCy.ToString();
                }
            }
        }
    }
}
