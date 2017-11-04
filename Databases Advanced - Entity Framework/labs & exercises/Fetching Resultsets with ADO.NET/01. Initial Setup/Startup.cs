using System;
using System.Data.SqlClient;

namespace _01._Initial_Setup
{
    class Startup
    {
        static void Main()
        {
            CreateDatabase();

            string connectionString = "Data Source=.;Initial Catalog=MinionsDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = @"CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))
                               CREATE TABLE Towns (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), CountryId INT NOT NULL, CONSTRAINT FK_TownCountry FOREIGN KEY (CountryId) REFERENCES Countries(Id))
                               CREATE TABLE Minions (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), Age INT, TownId INT, CONSTRAINT FK_Towns FOREIGN KEY (TownId) REFERENCES Towns(Id))
                               CREATE TABLE EvilnessFactors (Id INT PRIMARY KEY, Name VARCHAR(10) UNIQUE NOT NULL)
                               CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT, CONSTRAINT FK_VillainEvilnessFactor FOREIGN KEY (EvilnessFactorId) REFERENCES EvilnessFactors(Id))
                               CREATE TABLE MinionsVillains(MinionId INT, VillainId INT, CONSTRAINT FK_Minions FOREIGN KEY (MinionId) REFERENCES Minions(Id), CONSTRAINT FK_Villains FOREIGN KEY (VillainId) REFERENCES Villains(Id), CONSTRAINT PK_MinionsVillains PRIMARY KEY(MinionId, VillainId))

                               INSERT INTO Countries VALUES ('Bulgaria'), ('United Kingdom'), ('United States of America'), ('France')
                               INSERT INTO Towns (Name, CountryId) VALUES ('Sofia',1), ('Burgas',1), ('Varna', 1), ('London', 2),('Liverpool', 2),('Ocean City', 3),('Paris', 4)
                               INSERT INTO Minions (Name, Age, TownId) VALUES ('bob',10,1),('kevin',12,2),('stuart',9,3), ('rob',22,3), ('michael',5,2),('pep',3,2)
                               INSERT INTO EvilnessFactors VALUES (1, 'Super Good'), (2, 'Good'), (3, 'Bad'), (4, 'Evil'), (5, 'Super Evil')
                               INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru', 2),('Victor', 4),('Simon Cat', 3),('Pusheen', 1),('Mammal', 5)
                               INSERT INTO MinionsVillains VALUES (1, 2), (3, 1), (1, 3), (3, 3), (4, 1), (2, 2), (1, 1), (3, 4), (1, 4), (1, 5), (5, 1)";

                SqlCommand command = new SqlCommand(cmd, connection);

                connection.Open();
                int result = command.ExecuteNonQuery();
                Console.WriteLine(result);
            }
        }

        private static void CreateDatabase()
        {
            string connectionString = "Data Source=.;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string cmd = "CREATE DATABASE MinionsDB";
                SqlCommand command = new SqlCommand(cmd, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
