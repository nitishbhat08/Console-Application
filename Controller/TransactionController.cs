using Assignment_1.Exceptions;
using Assignment_1.Model;
using Assignment_1.Utilities;
using Facade;
using Library;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Assignment_1.Controller
{
    class TransactionController
    {
        private int CustomerID { get; set; }
        private const decimal ATM_WITHDRAWAL_FEE = 0.10m;
        private const decimal ACCOUNT_TRANSFER_FEE = 0.20m;
        private const char DEPOSIT = 'D';
        private const char WITHDRAW = 'W';
        private const char TRANSFER = 'T';
        private const char SERVICE = 'S';
        private readonly DateTime TransactionTime = DateTime.UtcNow;

        private string ConnectionString { get; }
        //Creating Transaction List
        public List<Transaction> Transactions { get; }
        //Establishing a connection and fetching all the Transaction Table data to the above list
        public TransactionController()
        {
            try
            {
                ConnectionString = Program.ConnectionString;
                using var connection = ConnectionString.CreateConnection();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * from [TransactionTable]";

               //Transactions = command.GetDataTable().Select().Select(x =>
               // new Transaction((int)x["TransactionID"], (char)x["TransactionType"], (int)x["AccountNumber"], (int)x["DestinationAccountNumber"], (decimal)x["Amount"], (string)x["Comment"], (DateTime)x["TransactionTimeUtc"])).ToList();
            }
            catch(SqlException)
            {
                Console.WriteLine("Transaction List is Empty");
            }
        }

        //Inserting transactions into the database when called.
        public void InsertTransaction(Transaction transaction)
        {
            try
            {
                using var connection = Program.ConnectionString.CreateConnection();
                connection.Open();
              
                var command = connection.CreateCommand();
                command.CommandText =
                    "INSERT INTO [TransactionTable] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc) VALUES (@TransactionType, @AccountNumber, @DestinationAccountNumber, @Amount, @Comment, @TransactionTimeUtc)";
                command.Parameters.AddWithValue("TransactionType", transaction.TransactionType);
                command.Parameters.AddWithValue("AccountNumber", transaction.AccountNumber);
                command.Parameters.AddWithValue("DestinationAccountNumber", transaction.DestinationAccountNumber);
                command.Parameters.AddWithValue("Amount", transaction.Amount);
                if (String.IsNullOrEmpty(transaction.Comment))
                {
                    command.Parameters.AddWithValue("@Comment", "NULL");
                }
                else
                    command.Parameters.AddWithValue("Comment", transaction.Comment);
                command.Parameters.AddWithValue("TransactionTimeUtc", transaction.TransactionTimeUtc);

                command.ExecuteNonQuery();
            }
            catch(SqlException)
            {
                Console.WriteLine("Transaction could not be inserted into the database.");
            }
        }

        //Implementation of ATM Feature - Deposit
        public void Deposit(decimal amount, int customerID, string accountType)
        {
            CustomerID = customerID;
            if (amount < 0)
            {
                Console.WriteLine("Please enter a positive amount.");
                new CustomerMenu(CustomerID).Run();
            }
            else 
            {
                var accountController = new AccountController();
                int accountNumber = accountController.GetAccountNumber(customerID);
                decimal balance = accountController.GetBalance(customerID, accountType);
                decimal updatedBalance = Decimal.Add(balance, amount);
                accountController.UpdateBalance(customerID, accountType, updatedBalance);
                string comment = "ATM Deposit.";
                //Creates a new transaction in the database with Transaction Type D
                Transaction transaction = new Transaction(DEPOSIT, accountNumber, accountNumber, amount, comment, TransactionTime);
                InsertTransaction(transaction);

                Console.WriteLine("$" + amount + " deposited into account " + accountNumber);
            }
            
        }

        //Implementation of ATM Feature - Withdraw
        public void WithDraw(decimal amount, int customerID, string accountType)
        {
            CustomerID = customerID;
            if (amount < 0)
            {
                Console.WriteLine("Please enter a positive amount.");
                new CustomerMenu(CustomerID).Run();                
            }
            else
            {
                var accountController = new AccountController();
                decimal balance = accountController.GetBalance(customerID, accountType);
                decimal updatedBalance = Decimal.Subtract(balance, amount);

                if (accountType == "C" && updatedBalance < 200.00m)
                {
                    Console.WriteLine("The minimum balance allowed in Checking Account is A$200. Please withdraw accordingly to maintain the minimum balance. Current Balance is: " + balance);
                    new CustomerMenu(CustomerID).Run();
                }

                if (accountType == "S" && updatedBalance < 0.00m)
                {
                    Console.WriteLine("The minimum balance allowed in Savings Account is A$0. Please withdraw accordingly to maintain the minimum balance. Current Balance is: " + balance);
                    new CustomerMenu(CustomerID).Run();
                }

                accountController.UpdateBalance(customerID, accountType, updatedBalance - ATM_WITHDRAWAL_FEE);
                int accountNumber = accountController.GetAccountNumber(customerID);
                string comment = "ATM Deposit.";

                //Creates a new transaction in the database with Transaction Type W
                Transaction transactionWithdraw = new Transaction(WITHDRAW, accountNumber, accountNumber, amount, comment, TransactionTime);
                InsertTransaction(transactionWithdraw);

                string serviceComment = "ATM Withdrawl Service Charge.";
                //Creates a new transaction in the database with Transaction Type S
                Transaction transactionServiceCharge = new Transaction(SERVICE, accountNumber, accountNumber, ATM_WITHDRAWAL_FEE, serviceComment, TransactionTime);
                InsertTransaction(transactionServiceCharge);

                Console.WriteLine("$" + amount + " with drawed from account " + accountNumber);
            }
        }

        public void AccountTransfer(decimal amount, int customerID, int destAccountNumber, string accountType)
        {
            CustomerID = customerID;
            if (amount < 0)
            {
                Console.WriteLine("Please enter a positive amount.");
                new CustomerMenu(CustomerID).Run();
            }
            else
            {
                var accountController = new AccountController();
                decimal balance = accountController.GetBalance(customerID, accountType);
                decimal updatedBalance = Decimal.Subtract(balance, amount);

                if (accountType == "C" && updatedBalance < 200.00m)
                {
                    Console.WriteLine("The minimum balance allowed in Checking Account is A$200. Please withdraw accordingly to maintain the minimum balance. Current Balance is: " + balance);
                    new CustomerMenu(CustomerID).Run();
                }

                if (accountType == "S" && updatedBalance < 0.00m)
                { 
                    Console.WriteLine("The minimum balance allowed in Savings Account is A$0. Please withdraw accordingly to maintain the minimum balance. Current Balance is: " + balance);
                    new CustomerMenu(CustomerID).Run();
                }

                accountController.UpdateBalance(customerID, accountType, updatedBalance - ACCOUNT_TRANSFER_FEE);
                accountController.UpdateDestAccountBalance(destAccountNumber, amount);
                int accountNumber = accountController.GetAccountNumber(customerID);

                string tranferCommentDebit = "Transfered to account: " + destAccountNumber;

                //Creates a new transaction in the database with Transaction Type T
                Transaction transactionTransfer = new Transaction(TRANSFER, accountNumber, destAccountNumber, amount, tranferCommentDebit, TransactionTime);
                InsertTransaction(transactionTransfer);

                string transferComment = "Received from account: " + accountNumber;

                //Creates a new transaction in the database with Transaction Type D
                Transaction transactionDeposit = new Transaction(TRANSFER, destAccountNumber, accountNumber, amount, transferComment, TransactionTime);
                InsertTransaction(transactionDeposit);

                string serviceComment = "Service Charge";

                //Creates a new transaction in the database with Transaction Type S
                Transaction transaction = new Transaction(SERVICE, accountNumber, destAccountNumber, amount, serviceComment, TransactionTime);
                InsertTransaction(transaction);

                Console.WriteLine("$" + amount + " transfered to account " + destAccountNumber);
            }
        }

        public void MyStatements()
        {
            SqlConnection con = new SqlConnection(ConnectionString);
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM TransactionTable", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow row in dt.Rows) 
            {
                Console.WriteLine(row["TransactionID"]);
                Console.WriteLine(row["AccountNumber"]);
                Console.WriteLine(row["DestinationAccountNumber"]);
                Console.WriteLine(row["Amount"]);
                Console.WriteLine(row["Comment"]);
                Console.WriteLine(row["TransactionTimeUtc"]);
                Console.WriteLine("***************************");
            }
        }
    }
}
