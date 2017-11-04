using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07._Print_All_Minion_Names
{
    class Startup
    {
        static void Main()
        {
            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = "SELECT [Name] FROM Minions";
                SqlCommand command = new SqlCommand(cmd, connection);

                List<string> minionNames = new List<string>();
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minionNames.Add(reader["Name"].ToString());
                    }
                }

                List<string> minionNamesOutput = new List<string>();
                int low = 0;
                int high = minionNames.Count - 1;

                while (low <= high)
                {
                    minionNamesOutput.Add(minionNames[low]);
                    if (low != high)
                        minionNamesOutput.Add(minionNames[high]);

                    low++;
                    high--;
                }

                string newLine = Environment.NewLine + "-- ";

                Console.WriteLine($"Original Order {newLine + string.Join(newLine, minionNames)}");
                Console.WriteLine();
                Console.WriteLine($"Output {newLine + string.Join(newLine, minionNamesOutput)}");
            }
        }
    }
}
