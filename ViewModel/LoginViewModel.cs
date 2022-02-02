using System;
using System.Windows.Input;
using Organizer.View.Windows;
using Organizer.Model;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;

namespace Organizer.ViewModel
{
    class LoginViewModel
    {
        private ICommand mSqlConfig;
        private ICommand mExit;
        private ICommand mLogin;
        public ICommand OpenConnectionConfig
        {
            get
            {
                if (mSqlConfig == null)
                    mSqlConfig = new ConfigClass();
                return mSqlConfig;
            }
            set
            {
                mSqlConfig = value;
            }
        }
        public ICommand Exit
        {
            get
            {
                if (mExit == null)
                    mExit = new ExitClass();
                return mExit;
            }
            set
            {
                mExit = value;
            }
        }
        public ICommand Login
        {
            get
            {
                mLogin = new LoginClass();
                return mLogin;
            }
            set
            {
                mLogin = value;
            }
        }
        private class ConfigClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                SQLConfigWindow sqlConfigWindow = new SQLConfigWindow();
                sqlConfigWindow.Show();
            }
        }
        private class ExitClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                LoginWindow window = (LoginWindow)parameter;
                window.Close();
            }
        }
        private class LoginClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                LoginWindow window = (LoginWindow)parameter;
                LoginViewModel lvm = new LoginViewModel();
                User user = lvm.UserCheck(window.LoginInput.Text, window.PasswordInput.Password);
                if (user != null)
                {
                    string loginPath = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\LoginUser.xml");
                    XDocument login = XDocument.Load(loginPath);
                    var xLogin = login.Root;
                    xLogin.Element("ID").Value = user.ID.ToString();
                    xLogin.Element("Login").Value = user.Login.ToString();
                    xLogin.Element("Password").Value = user.Password.ToString();
                    xLogin.Element("Name").Value = user.Name.ToString();
                    xLogin.Element("Surname").Value = user.Surname.ToString();
                    xLogin.Element("Email").Value = user.Email.ToString();
                    xLogin.Element("PhoneNumber").Value = user.PhoneNumber.ToString();
                    xLogin.Save(loginPath);

                    string path = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\DBConfig.xml");
                    XDocument config = XDocument.Load(path);
                    var x = config.Root;

                    MainAppWindow mainAppWindow = new MainAppWindow();
                    mainAppWindow.WindowState = System.Windows.WindowState.Maximized;
                    mainAppWindow.Title = "Organizer";
                    mainAppWindow.LoginInfo.Text = "Zalogowany operator: (" + user.ID + ") " + user.Name + " " + user.Surname;
                    mainAppWindow.DataBaseInfo.Text = "Baza danych: " + x.Element("sqlDataBase").Value;
                    mainAppWindow.Show();
                    window.Close();
                }
            }
        }
        public User UserCheck(string login, string password)
        {

            User matchingUser = new User(login, password);
            User resultUser = null;
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string oString = "Select * from Users where Login=@Login";
                SqlCommand oCmd = new SqlCommand(oString, myConnection);
                myConnection.Open();
                oCmd.Parameters.AddWithValue("@Login", login);
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        if (
                            matchingUser.Login == oReader["Login"].ToString().Trim() &&
                            matchingUser.Password == oReader["Password"].ToString().Trim())
                        {
                            resultUser = new User(
                                Convert.ToInt32(oReader["ID"].ToString().Trim()),
                                oReader["Login"].ToString().Trim(),
                                oReader["Password"].ToString().Trim(),
                                oReader["Name"].ToString().Trim(),
                                oReader["Surname"].ToString().Trim(),
                                oReader["Email"].ToString().Trim(),
                                oReader["Phone"].ToString().Trim()
                                );
                        }
                    }
                    myConnection.Close();
                }
            }
            return resultUser;
        }
    }
}
