using System;
using System.Xml.Linq;
using System.IO;
using System.Windows.Input;
using Organizer.View.Windows;
using Organizer.SQLConnection;
using System.Windows;

namespace Organizer.ViewModel
{
    class SQLConfigViewModel
    {
        public string SqlServer { get; set; }
        public string DBName { get; set; }
        public string SQLLogin { get; set; }
        public string SQLPassword { get; set; }
        public SQLConfigViewModel()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\DBConfig.xml");
            XDocument config = XDocument.Load(path);
            var xConfig = config.Root;
            this.SqlServer = xConfig.Element("sqlServer").Value;
            this.DBName = xConfig.Element("sqlDataBase").Value;
            this.SQLLogin = xConfig.Element("login").Value;
            this.SQLPassword = xConfig.Element("password").Value;
        }
        private ICommand mSave;
        private ICommand mTest;
        public ICommand SaveConfiguration
        {
            get
            {
                mSave = new SaveClass();
                return mSave;
            }
            set
            {
                mSave = value;
            }
        }
        public ICommand TestSqlConnection
        {
            get
            {
                mTest = new TestClass();
                return mTest;
            }
            set
            {
                mTest = value;
            }

        }
        private class SaveClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                string path = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\DBConfig.xml");
                XDocument config = XDocument.Load(path);
                var xConfig = config.Root;
                SQLConfigWindow window = (SQLConfigWindow)parameter;
                xConfig.Element("sqlServer").Value = window.SqlServerInput.Text;
                xConfig.Element("sqlDataBase").Value = window.DBNameInput.Text;
                xConfig.Element("login").Value = window.SQLLoginInput.Text;
                xConfig.Element("password").Value = window.SQLPasswordInput.Text;
                config.Save(path);
                window.Close();
            }
        }
        private class TestClass : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                if (ConnectionTest.SQLConnectionTest())
                {
                    MessageBox.Show("Połączenie OK");
                }
                else
                {
                    MessageBox.Show("Błąd połączenia");
                }
            }
        }
    }
}
