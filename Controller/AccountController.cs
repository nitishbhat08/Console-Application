using Assignment_1.Exceptions;
using Assignment_1.Model;
using Assignment_1.Utilities;
using Facade;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_1.Controller
{
    class AccountController
    {
        private string ConnectionString { get; }
        private decimal balance;
        private int accountNumber;

        //Creating a list of Accounts
        public List<Account> Accounts { get; }

        //Establishing a connection and fetching all the Accounts Table data to the above list
        public AccountController()
        {
            try
            {
                ConnectionString = Program.ConnectionString; //Gets Connection String using the Facade Pattern

                using var connection = ConnectionString.CreateConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Account";

                Accounts = command.GetDataTable().Select().Select(x =>
                new Account((int)x["AccountNumber"], (string)x["AccountType"], (int)x["CustomerID"], (decimal)x["Balance"])).ToList();
            }
            catch(SqlException)
            {
                Console.WriteLine("Account List is empty.");
            }            
        }

        //Fetches Account Number from the database
        public int GetAccountNumber(int customerID)
        {
            try
            {
                using var connection = ConnectionString.CreateConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT AccountNumber FROM Account WHERE CustomerID = @customerID;";
                command.Parameters.AddWithValue("customerID", customerID);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        accountNumber = ((int)reader["AccountNumber"]);
                    }
                }
                reader.Close();
            }
            catch(SqlException)
            {
                Console.WriteLine("Account Number not found in the database.");
            }
            return accountNumber;
        }

        //Fetches Balance from the Account database
        public decimal GetBalance(int customerID, string accountType)
        {
            try
            {
                using var connection = ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT Balance FROM Account WHERE CustomerID = @customerID AND AccountType = @accountType;";
                command.Parameters.AddWithValue("AccountType", accountType);
                command.Parameters.AddWithValue("CustomerID", customerID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        balance = ((decimal)reader["Balance"]);
                    }
                }
                else
                {
                    Console.WriteLine("Please choose correct Account Type (S or C).");
                    new CustomerMenu(customerID).Run();
                }
                reader.Close();
            }
            catch(SqlException)
            {
                Console.WriteLine("Account Balance not found in the database.");
            }
            return balance;
        }

        //Updates Balance in the Account Database
        public void UpdateBalance(int customerID, string accountType, decimal balance)
        {
            try
            {
                using var connection = ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE Account SET Balance = @balance WHERE AccountType = @accountType AND CustomerID = @customerID";
                command.Parameters.AddWithValue("Balance", balance);
                command.Parameters.AddWithValue("AccountType", accountType);
                command.Parameters.AddWithValue("CustomerID", customerID);

                command.ExecuteNonQuery();

                Console.WriteLine("Balance Updated");
            }
            catch(SqlException)
            {
                Console.WriteLine("Account Balance could not be updated.");
            }
        }

        //Updates Balance in the Destination Account Database
        public void UpdateDestAccountBalance(int accountNumber, decimal balance)
        {
            try
            {
                using var connection = ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE Account SET Balance = @balance WHERE AccountNumber = @accountNumber";
                command.Parameters.AddWithValue("Balance", balance);
                command.Parameters.AddWithValue("AccountNumber", accountNumber);

                command.ExecuteNonQuery();

                Console.WriteLine("Destination Account Balance Updated");
            }
            catch (SqlException)
            {
                Console.WriteLine("Destination Account Balance could not be updated.");
            }
        }
    }
}
