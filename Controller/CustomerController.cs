using Assignment_1.Model;
using Assignment_1.Utilities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment_1.Controller
{
    class CustomerController
    {
        private string ConnectionString { get; }
        //Creating a list of Customers
        public List<Customer> Customers { get; }
        //Establishing a connection and fetching all the Customer Table data to the above list
        public CustomerController(string connectionString)
        {
            try
            {
                ConnectionString = connectionString;
                using var connection = ConnectionString.CreateConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Customer";

                Customers = command.GetDataTable().Select().Select(x =>
                    new Customer((int)x["CustomerID"], (string)x["Name"], (string)x["Address"], (string)x["City"], (string)x["PostCode"])).ToList();
            }
            catch(SqlException)
            {
                Console.WriteLine("Customer List is Empty");
            }
        }
    }
}