using System;

namespace Assignment_1.Model
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public char TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int DestinationAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionTimeUtc { get; set; }

        public Transaction(int transactionID, char transactionType, int accountNumber, int destinationAccountNumber, decimal amount, string comment, DateTime transactionTimeUtc)
        {
            TransactionID = transactionID;
            TransactionType = transactionType;
            AccountNumber = accountNumber;
            DestinationAccountNumber = destinationAccountNumber;
            Amount = amount;
            Comment = comment;
            TransactionTimeUtc = transactionTimeUtc;
        }

        public Transaction(char transactionType, int accountNumber, int destinationAccountNumber, decimal amount, string comment, DateTime transactionTimeUtc)
        {
            TransactionType = transactionType;
            AccountNumber = accountNumber;
            DestinationAccountNumber = destinationAccountNumber;
            Amount = amount;
            Comment = comment;
            TransactionTimeUtc = transactionTimeUtc;
        }

        public Transaction(){}
    }
}
