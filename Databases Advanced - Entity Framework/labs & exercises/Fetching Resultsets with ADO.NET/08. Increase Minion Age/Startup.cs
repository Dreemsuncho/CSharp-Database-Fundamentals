using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace _08._Increase_Minion_Age
{
    class Startup
    {
        static void Main()
        {
            List<int> minionIds = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToList();

            string connectionString = "Data Source=.;Initial Catalog=MinionsDb;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = $"UPDATE Minions SET Age = Age + 1, [Name] = CONCAT(LEFT(UPPER([Name]), 1), RIGHT([Name], LEN([Name]) -1)) WHERE Id IN ({string.Join(",", minionIds)})";
                SqlCommand command = new SqlCommand(cmd, connection);

                connection.Open();
                command.ExecuteNonQuery();

                cmd = "SELECT [Name], Age FROM Minions";
                command.CommandText = cmd;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["Age"]}");
                    }
                }
            }
        }
    }
}