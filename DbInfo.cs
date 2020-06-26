using System;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class DbInfo
    {
        static string connectionString = "Data Source=WINDOWS-A07GTAC\\SQLEXPRESS;Initial Catalog=Learning;Integrated Security=false;";
        public void LoginPassword(string username= "user", string password= "Vazgen123#")
        {
            connectionString += String.Format("User Id = {0}; Password = {1};", username, password);
        }
        public SqlConnection OpenConnection()
        {
            LoginPassword();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;
            sqlConnection.Open();
            return sqlConnection;
        }
        public SqlConnection ForLogin()
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}