using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05._Change_Town_Names_Casing
{
    class Startup
    {
        static void Main()
        {
            Console.WriteLine("Enter Country Name:");
            string countryName = Console.ReadLine();

            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string cmd = "UPDATE Towns SET [Name] = UPPER([Name]) WHERE CountryId = (SELECT Id FROM Countries WHERE [Name] = @CountryName)";
                SqlCommand command = new SqlCommand(cmd, connection);
                command.Parameters.AddWithValue("@CountryName", countryName);

                connection.Open();
                int affectedTownsCount = command.ExecuteNonQuery();

                if (affectedTownsCount > 0)
                {
                    Console.WriteLine($"{affectedTownsCount} town names were affected.");

                    cmd = "SELECT [Name] FROM Towns WHERE CountryId = (SELECT Id FROM Countries WHERE [Name] = @CountryName)";
                    command.CommandText = cmd;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> towns = new List<string>();
                        while (reader.Read())
                        {
                            towns.Add(reader["Name"].ToString());
                        }

                        Console.WriteLine($"[{string.Join(", ", towns)}]");
                    }
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }
            }
        }
    }
}
