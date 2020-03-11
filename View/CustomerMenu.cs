using Assignment_1.Controller;
using Assignment_1.Model.Menus;
using Facade;
using System;

namespace Assignment_1
{
    public class CustomerMenu
    {
        private int CustomerID { get; set; }
        private decimal Amount { get; set; }
        private string AccountType { get; set; }
        private int DestAccountNumber { get; set; }
        public CustomerMenu(int customerID)
        {
            CustomerID = customerID;
        }

        public void Run()
        {
            while(true)
            {
                Console.Write(
"\nNational Wealth Bank of Australia"+
"\n***************************************"+
"\n" +
"\nLogged in as " +CustomerID+
"\n"+
"\n1. Withdraw" +
"\n2. Deposit"+
"\n3. Account Transfer"+
"\n4. My Statements"+
"\n5. Logout"+
"\n6. Exit"+
"\n"+
"\nPlease select an option: ");

               var input = Console.ReadLine();
                Console.WriteLine();

                if(!int.TryParse(input, out var option) || option < 1 || option > 6)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch(option)
                {
                    case 1:
                        Console.WriteLine("Enter amount to be withdrawed: ");
                        Amount = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Account Type C or S: ");
                        AccountType = Console.ReadLine();
                        new TransactionController().WithDraw(Amount, CustomerID, AccountType);
                        break;

                    case 2:
                        Console.WriteLine("Enter Deposit amount: ");
                        Amount = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Account Type C or S: ");
                        AccountType = Console.ReadLine();
                        new TransactionController().Deposit(Amount, CustomerID, AccountType);
                        break;

                    case 3:
                        Console.WriteLine("Enter amount to be Transfered: ");
                        Amount = Convert.ToDecimal(Console.ReadLine());
                        Console.WriteLine("Enter Destination Account Number: ");
                        DestAccountNumber = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Choose Account Type C or S: ");
                        AccountType = Console.ReadLine();
                        new TransactionController().AccountTransfer(Amount, CustomerID, DestAccountNumber, AccountType);
                        break;

                    case 4:
                        new TransactionController().MyStatements();
                        break;

                    case 5:
                        Console.Clear();
                        new MainMenu(Program.ConnectionString).Run();
                        break;

                    case 6:
                        Console.WriteLine("Program Ends");
                        Environment.Exit(0);
                        break;
                        
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
