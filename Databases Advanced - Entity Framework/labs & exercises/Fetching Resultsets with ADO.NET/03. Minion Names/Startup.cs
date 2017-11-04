using System;
using System.Data.SqlClient;

namespace _03._Minion_Names
{
    class Startup
    {
        static void Main()
        {
            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Enter a villain Id:");
                int villainId = int.Parse(Console.ReadLine());

                string cmd = "SELECT [Name] FROM Villains WHERE Id = @VillainId";
                SqlCommand command = new SqlCommand(cmd, connection);
                command.Parameters.AddWithValue("@VillainId", villainId);

                connection.Open();
                using (SqlDataReader villainReader = command.ExecuteReader())
                {
                    if (villainReader.HasRows)
                    {
                        villainReader.Read();
                        Console.WriteLine($"Villain: {villainReader["Name"]}");
                    }
                    else
                    {
                        Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                        return;
                    }
                }


                cmd = @"SELECT m.[Name], m.Age 
                        FROM Minions m
                        JOIN MinionsVillains mv ON mv.VillainId = @VillainId AND mv.MinionId = m.Id";
                command.CommandText = cmd;
                using (SqlDataReader minionsReader = command.ExecuteReader())
                {
                    int rowCount = 1;
                    while (minionsReader.HasRows)
                    {
                        while (minionsReader.Read())
                        {
                            Console.WriteLine($"{rowCount++}. {minionsReader["Name"]} {minionsReader["Age"]}");
                        }
                        if (rowCount == 1)
                        {
                            Console.WriteLine("(no minions)");
                        }

                        minionsReader.NextResult();
                    }
                }
            }
        }
    }
}
