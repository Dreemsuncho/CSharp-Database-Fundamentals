/* Problem 1. Create Database
You now know how to create database using the GUI of the SSMS. Now it’s time to create it using SQL queries. In that task (and the several following it) you will be required to create the database from the previous exercise using only SQL queries. Firstly, just create new database named Minions.
*/

USE Minions
CREATE TABLE Minions(Id INT,
					 Name NVARCHAR(15),
					 Age INT)
