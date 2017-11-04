using System;
using System.Data;
using System.Data.SqlClient;

namespace _09._Increase_Age_Stored_Procedure
{
    class Startup
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = "usp_GetOlder";
                Console.WriteLine("Add minion ID:");
                int minionId = int.Parse(Console.ReadLine());

                SqlCommand command = new SqlCommand(cmd, connection);
                command.Parameters.AddWithValue("@MinionId", minionId);

                connection.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();


                cmd = "SELECT [Name], Age FROM Minions WHERE Id = @MinionId";
                command.CommandText = cmd;
                command.CommandType = CommandType.Text;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["Name"]} - {reader["Age"]} years old");
                    }
                }
            }
        }
    }
}
