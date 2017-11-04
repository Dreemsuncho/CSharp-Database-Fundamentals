using System;
using System.Data.SqlClient;

namespace _04._Add_Minion
{
    class Startup
    {
        private const string _connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";

        static void Main()
        {
            string[] minionInfoArray = Console.ReadLine().Split();
            string minionName = minionInfoArray[1];
            int minionAge = int.Parse(minionInfoArray[2]);
            string minionTown = minionInfoArray[3];

            string[] villainInfoArray = Console.ReadLine().Split();
            string villainName = villainInfoArray[1];

            bool hasTown = true;
            bool hasVillain = true;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    CheckExistingEntities(connection, transaction, minionTown, villainName, ref hasTown, ref hasVillain);

                    if (!hasTown)
                        InsertTown(connection, transaction, minionTown);

                    if (!hasVillain)
                        InsertVillain(connection, transaction, villainName);

                    MakeMinionServantOfVillain(connection, transaction, minionName, villainName);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }

        private static void MakeMinionServantOfVillain(SqlConnection connection, SqlTransaction transaction, string minionName, string villainName)
        {
            string cmd = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ((SELECT Id FROM Minions WHERE [Name] = @MinionName), (SELECT Id FROM Villains WHERE [Name] = @VillainName))";
            SqlCommand command = new SqlCommand(cmd, connection, transaction);
            command.Parameters.AddWithValue("@MinionName", minionName);
            command.Parameters.AddWithValue("@VillainName", villainName);

            if (command.ExecuteNonQuery() > 0)
                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }

        private static void InsertVillain(SqlConnection connection, SqlTransaction transaction, string villainName)
        {
            string cmd = "INSERT INTO Villains ([Name], EvilnessFactorId) VALUES (@VillainName, (SELECT Id FROM EvilnessFactors WHERE [Name] = 'evil'))";
            SqlCommand command = new SqlCommand(cmd, connection, transaction);
            command.Parameters.AddWithValue("@VillainName", villainName);

            if (command.ExecuteNonQuery() > 0)
                Console.WriteLine($"Villain {villainName} was added to the database.");
        }

        private static void InsertTown(SqlConnection connection, SqlTransaction transaction, string minionTown)
        {
            string cmd = "INSERT INTO Towns ([Name], CountryId) VALUES (@TownName, 1)";
            SqlCommand command = new SqlCommand(cmd, connection, transaction);
            command.Parameters.AddWithValue("@TownName", minionTown);

            if (command.ExecuteNonQuery() > 0)
                Console.WriteLine($"Town {minionTown} was added to the database.");
        }

        private static void CheckExistingEntities(SqlConnection connection, SqlTransaction transaction,
            string minionTown, string villainName, ref bool hasTown, ref bool hasVillain)
        {
            string cmd = "SELECT * FROM Towns WHERE [Name] = @TownName";
            SqlCommand command = new SqlCommand(cmd, connection, transaction);
            command.Parameters.AddWithValue("@TownName", minionTown);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    hasTown = false;
            }

            cmd = "SELECT * FROM Villains WHERE [Name] = @VillainName";
            command.CommandText = cmd;
            command.Parameters.AddWithValue("@VillainName", villainName);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (!reader.HasRows)
                    hasVillain = false;
            }
        }
    }
}
