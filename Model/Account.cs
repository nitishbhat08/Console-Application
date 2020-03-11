using System.Collections.Generic;

namespace Assignment_1.Model
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int CustomerID { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Account(int accountNumber, string accountType, int customerID, decimal balance)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            CustomerID = customerID;
            Balance = balance;
        }
    }
}
