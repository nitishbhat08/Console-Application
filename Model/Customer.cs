﻿using System.Collections.Generic;

namespace Assignment_1.Model
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public List<Account> Accounts { get; set; }

        public Customer(int customerID, string name, string address, string city, string postCode)
        {
            CustomerID = customerID;
            Name = name;
            Address = address;
            City = city;
            PostCode = postCode;
        }
    }
}