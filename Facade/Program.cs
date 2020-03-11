using Assignment_1.Model.Menus;
using Microsoft.Extensions.Configuration;
using Library; //Reference to the assembly file 

namespace Facade
{
    public static class Program
    {
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        //public const string ConnectionString
        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        public static void Main()
        {
            //Json FIles fetched into the database
            Library.Facade.GetAndSaveCustomers(ConnectionString);
            Library.Facade.GetAndSaveLogins(ConnectionString);

            //Calling the Main Menu
            new MainMenu(ConnectionString).Run();
        }
    }
}
