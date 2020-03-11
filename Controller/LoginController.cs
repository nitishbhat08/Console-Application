using Assignment_1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using SimpleHashing;
using Microsoft.Data.SqlClient;
using Assignment_1.Model;
using Assignment_1.Exceptions;

namespace Assignment_1.Controller
{
    class LoginController
    {
        private string ConnectionString { get; }
        //Creating Login List
        public List<Model.Login> Logins { get; }        
        //Establishing a connection and fetching all the Login Table data to the above list
        public LoginController(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
                using var connection = ConnectionString.CreateConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Login";

                Logins = command.GetDataTable().Select().Select(x =>
                    new Login((string)x["LoginID"], (int)x["CustomerID"], (string)x["PasswordHash"])).ToList();
            }
            catch(SqlException)
            {
                Console.WriteLine("Login List is Empty.");
            }
        }
        //Validates the login details entered by the user
        public void ValidateLogin(string loginID, string password)
        {
            try
            {
                using var connection = ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Login WHERE LoginID = @loginID;"; //Validates login ID with the database
                command.Parameters.AddWithValue("loginID", loginID);

                SqlDataReader reader = command.ExecuteReader();

                bool key;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var passwordH = (string)reader["PasswordHash"];
                        key = PBKDF2.Verify(passwordH, password);  //Compares the hashed password in the database with the passsword entered by the user. Returns true if the password is correct.
                        if (key)
                        {
                            new CustomerMenu((int)reader["CustomerID"]).Run();
                        }
                        else
                        {
                             //Prompts back to the Main menu if wrong password is entered.
                            Console.WriteLine("\nInvalid Password, please log in with correct password.\n");
                            break;
                        }
                    }
                }
                else
                {
                    //Prompts back to the Main menu if wrong login id is entered.
                    Console.WriteLine("\nInvalid Login ID, please log in with correct login ID.\n");
                }
                reader.Close();
            }
            catch (SqlException)
            {
                Console.WriteLine("Please check connection with database.");
            }
        }
    }
}
