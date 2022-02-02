using System;
using Organizer.View.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Input;
using System.Xml.Linq;

namespace Organizer.ViewModel
{
    class MainAppViewModel
    {
        private ICommand mLogout;
        private ICommand mShoppingCard;
        public string ImageSource { get; set; }
        public MainAppViewModel()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"Graphics\home.jpg");
            ImageSource = path;
        }
        public ICommand Logout
        {
            get
            {
                mLogout = new LogoutClass();
                return mLogout;
            }
            set
            {
                mLogout = value;
            }
        }
        public ICommand ShowShoppingCard
        {
            get
            {
                mShoppingCard = new ShoppingCardClass();
                return mShoppingCard;
            }
            set
            {
                mShoppingCard = value;
            }
        }
        private class LogoutClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                string loginPath = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\LoginUser.xml");
                XDocument login = XDocument.Load(loginPath);
                var xLogin = login.Root;
                xLogin.Element("ID").Value = string.Empty;
                xLogin.Element("Login").Value = string.Empty;
                xLogin.Element("Password").Value = string.Empty;
                xLogin.Element("Name").Value = string.Empty;
                xLogin.Element("Surname").Value = string.Empty;
                xLogin.Element("Email").Value = string.Empty;
                xLogin.Element("PhoneNumber").Value = string.Empty;
                xLogin.Save(loginPath);

                MainAppWindow window = (MainAppWindow)parameter;
                LoginWindow lWindow = new LoginWindow();
                lWindow.Show();
                window.Close();
            }
        }
        private class ShoppingCardClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                ContentControl cc = (ContentControl)parameter;
                cc.DataContext = new ShoppingCardViewModel();
            }
        }
    }
}
