using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Assignment_1.Utilities
{
    public static class MiscExtensions
    {
        public static SqlConnection CreateConnection(this string connectionString) =>
            new SqlConnection(connectionString);

        public static DataTable GetDataTable(this SqlCommand command)
        {
            var table = new DataTable();
            new SqlDataAdapter(command).Fill(table);

            return table;
        }
    }
}
