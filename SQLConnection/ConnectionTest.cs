using System;
using System.Data.SqlClient;

namespace Organizer.SQLConnection
{
    class ConnectionTest
    {
        public static bool SQLConnectionTest()
        {
            bool test = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString.ConnectString()))
            {
                try
                {
                    connection.Open();
                    test = true;
                    connection.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return test;
        }
    }
}
