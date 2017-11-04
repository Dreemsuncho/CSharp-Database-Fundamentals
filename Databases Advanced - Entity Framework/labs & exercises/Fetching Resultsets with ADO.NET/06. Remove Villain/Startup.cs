using System;
using System.Data.SqlClient;

namespace _06._Remove_Villain
{
    class Startup
    {
        static void Main()
        {
            Console.WriteLine("Enter villain ID:");
            int villainId = int.Parse(Console.ReadLine());

            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string cmd = "SELECT [Name] FROM Villains WHERE Id = @VillainID";
                    SqlCommand command = new SqlCommand(cmd, connection, transaction);
                    command.Parameters.AddWithValue("@VillainId", villainId);

                    string villainName = null;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No such villain was found.");
                            return;
                        }
                        reader.Read();
                        villainName = reader["Name"].ToString();
                    }

                    cmd = "DELETE FROM MinionsVillains WHERE VillainId = @VillainId";
                    command.CommandText = cmd;

                    int releasedMinionsCount = command.ExecuteNonQuery();

                    cmd = "DELETE FROM Villains WHERE Id = @VillainId";
                    command.CommandText = cmd;
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{releasedMinionsCount} minions were released.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
