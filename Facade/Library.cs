using Assignment_1.Controller;
using Assignment_1.Model;
using Assignment_1.Utilities;
using Facade;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Library
{
    internal class Inserts
    {
        internal void InsertLogin(Login login)
        {
            try
            {
                using var connection = Program.ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Login (LoginID, CustomerID, PasswordHash) VALUES (@LoginID, @CustomerID, @PasswordHash)";
                command.Parameters.AddWithValue("LoginID", login.LoginID);
                command.Parameters.AddWithValue("CustomerID", login.CustomerID);
                command.Parameters.AddWithValue("PasswordHash", login.PasswordHash);

                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Console.WriteLine("Login details not inserted");
            }
        }

        internal void InsertAccount(Account account)
        {
            try
            {
                using var connection = Program.ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Account (AccountNumber, AccountType, CustomerID, Balance) VALUES (@AccountNumber, @AccountType, @CustomerID, @Balance)";
                command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
                command.Parameters.AddWithValue("AccountType", account.AccountType);
                command.Parameters.AddWithValue("CustomerID", account.CustomerID);
                command.Parameters.AddWithValue("Balance", account.Balance);

                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Console.WriteLine("Accounts not inserted");
            }
        }
        internal void InsertCustomer(Customer customer)
        {
            try
            {
                using var connection = Program.ConnectionString.CreateConnection();
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Customer(CustomerID, Name, Address, City, PostCode) VALUES (@CustomerID, @Name, @Address, @City, @PostCode)";
                command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
                command.Parameters.AddWithValue("Name", customer.Name);

                if (String.IsNullOrEmpty(customer.Address))
                {
                    command.Parameters.AddWithValue("@Address", "NULL");
                }
                else
                    command.Parameters.AddWithValue("@Address", customer.Address);

                if (String.IsNullOrEmpty(customer.City))
                {
                    command.Parameters.AddWithValue("@City", "NULL");
                }
                else
                    command.Parameters.AddWithValue("@City", customer.City);

                if (String.IsNullOrEmpty(customer.PostCode))
                {
                    command.Parameters.AddWithValue("@PostCode", "NULL");
                }
                else
                    command.Parameters.AddWithValue("@PostCode", customer.PostCode);

                command.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Console.WriteLine("Customer not inserted");
            }
        }
    }
    public static class Facade
    {
        //Reference to internal methods created
        static Inserts a = new Inserts();

        //Refered from Day 03 Tute Examples
        public static void GetAndSaveCustomers(string connectionString)
        {
            try
            {
                var customerController = new CustomerController(connectionString);
                if (customerController.Customers.Any())
                    return;

                using var client = new HttpClient();

                var json = client.GetStringAsync("http://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;
                var customers = JsonConvert.DeserializeObject<List<Customer>>(json,
                    new JsonSerializerSettings { DateFormatString = "dd/MM/yyyy hh:mm:ss tt" });

                foreach (var customer in customers)
                {
                    a.InsertCustomer(customer);
                    System.Console.WriteLine("Customers Inserted");
                    foreach (var account in customer.Accounts)
                    {
                        a.InsertAccount(account);
                        Console.WriteLine("Account Details Inserted");
                        foreach (var transaction in account.Transactions)
                        {
                            char transactionType = 'D';
                            Transaction transactions = new Transaction(transactionType, account.AccountNumber, account.AccountNumber, account.Balance, transaction.Comment, transaction.TransactionTimeUtc);
                            TransactionController transactionController = new TransactionController();
                            transactionController.InsertTransaction(transaction);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                Console.WriteLine("Fetching error from JSON File");
            }
        }

        //Refered from Day 03 Tute Examples
        public static void GetAndSaveLogins(string connectionString)
        {
            try
            {
                var loginController = new LoginController(connectionString);
                if (loginController.Logins.Any())
                    return;

                using var client = new HttpClient();
                var json =
                    client.GetStringAsync("http://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").Result;
                var logins = JsonConvert.DeserializeObject<List<Login>>(json);

                foreach (var login in logins)
                {
                    a.InsertLogin(login);
                }
                Console.WriteLine("Login Details Added.");
            }
            catch (SqlException)
            {
                Console.WriteLine("Fetching error from JSON File");
            }
        }
    }
}