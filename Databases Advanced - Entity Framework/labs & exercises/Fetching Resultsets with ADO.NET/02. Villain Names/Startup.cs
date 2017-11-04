using System;
using System.Data.SqlClient;

namespace _02._Villain_Names
{
    class Startup
    {
        static void Main()
        {
            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = @"SELECT v.[Name], COUNT(mv.MinionId) AS [Minions Count]
                               FROM Villains v
                               JOIN MinionsVillains mv ON mv.VillainId = v.Id
                               GROUP BY v.[Name]
                               HAVING COUNT(mv.MinionId) >= 3
                               ORDER BY [Minions Count] DESC";

                SqlCommand command = new SqlCommand(cmd, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["Name"]} - {reader["Minions Count"]}");
                        }
                        reader.NextResult();
                    }
                }
            }
        }
    }
}
