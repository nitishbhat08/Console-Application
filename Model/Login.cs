namespace Assignment_1.Model
{
    public class Login
    {
        public string LoginID { get; set; }
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; }
        public Login(string loginID, int customerID, string passwordHash)
        {
            LoginID = loginID;
            CustomerID = customerID;
            PasswordHash = passwordHash;
        }
    }
}
