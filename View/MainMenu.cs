using Assignment_1.Controller;
using System;

namespace Assignment_1.Model.Menus
{
    public class MainMenu
    {
        private LoginController LoginController { get; }

        public MainMenu(string connectionString)
        {
            LoginController = new LoginController(connectionString);

        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("National Wealth Bank of Australia (NWBA)");
                Console.WriteLine("*********************************");
                Console.WriteLine("1. Customer Login");
                Console.WriteLine("2. Quit");
                Console.WriteLine();
                Console.Write("Enter an option: ");

                var input = Console.ReadLine();
                Console.WriteLine();

                if (!int.TryParse(input, out var option) || option < 1 || option > 2)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Console.WriteLine("Login ID: ");
                        string loginID = Console.ReadLine();
                        Console.WriteLine("Password: ");
                        string password = Console.ReadLine();
                        LoginController.ValidateLogin(loginID,password);
                        break;

                    case 2:
                        Console.WriteLine("Quitting..");
                        return;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
