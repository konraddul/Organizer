using System;
using System.Xml.Linq;
using System.IO;

namespace Organizer.SQLConnection
{
    class ConnectionString
    {
        public static string ConnectString()
        {
            string path = Path.Combine(Environment.CurrentDirectory, @"SQLConnection\Xmls\DBConfig.xml");
            XDocument config = XDocument.Load(path);
            var x = config.Root;
            return @"Data Source=" + x.Element("sqlServer").Value + ";Initial Catalog=" + x.Element("sqlDataBase").Value + ";User ID=" + x.Element("login").Value + ";Password=" + x.Element("password").Value;
        }
    }
}
