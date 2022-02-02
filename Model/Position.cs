using System;
using System.Collections.Generic;
using System.Text;
using Organizer.Types;
using System.Data.SqlClient;

namespace Organizer.Model
{
    public class Position
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Shop { get; set; }
        public string Product { get; set; }
        public Currency Amount { get; set; }
        public Category Category { get; set; }
        public User Operator { get; set; }
        public string DateS { get; set; }
        public Position(int id, DateTime date, string shop, string product, decimal amount, string currency, string category, string oper)
        {
            this.ID = id;
            this.Date = date;
            this.Shop = shop;
            this.Product = product;
            this.Amount = new Currency(amount, currency);
            this.Category = GetCategory(category);
            this.Operator = GetUser(oper);
            this.DateS = date.ToString("dd-MMMM-yyyy");
        }
        Category GetCategory(string category)
        {
            Category cat = null;
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string query = "Select * From Categories Where Name=@Name";
                SqlCommand command = new SqlCommand(query, myConnection);
                myConnection.Open();
                command.Parameters.AddWithValue("@Name", category);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        cat = new Category(
                            Convert.ToInt32(reader["ID"].ToString().Trim()),
                            reader["Name"].ToString().Trim(),
                            Convert.ToDecimal(reader["Limit"].ToString().Trim()),
                            reader["Currency"].ToString().Trim()
                            );
                    }
                    myConnection.Close();
                }
            }
            return cat;
        }
        User GetUser(string oper)
        {
            User user = null;
            using (SqlConnection myConnection = new SqlConnection(SQLConnection.ConnectionString.ConnectString()))
            {
                string query = "Select * From Users Where Login=@Login";
                SqlCommand command = new SqlCommand(query, myConnection);
                myConnection.Open();
                command.Parameters.AddWithValue("@Login", oper);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User(
                            Convert.ToInt32(reader["ID"].ToString().Trim()),
                            reader["Login"].ToString().Trim(),
                            reader["Password"].ToString().Trim(),
                            reader["Name"].ToString().Trim(),
                            reader["Surname"].ToString().Trim(),
                            reader["Email"].ToString().Trim(),
                            reader["Phone"].ToString().Trim()
                            );
                    }
                    myConnection.Close();
                    myConnection.Dispose();
                }
            }
            return user;
        }
        public override string ToString()
        {
            return this.Product + " - " + this.Amount.ToString() + " " + this.DateS;
        }
    }
}
