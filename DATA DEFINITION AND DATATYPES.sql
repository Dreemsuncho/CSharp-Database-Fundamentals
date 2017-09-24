/* Problem 1 - Create Database.
-- You now know how to create database using the GUI of the SSMS. Now it’s time to create it using SQL queries. In that task (and the several following it) you will be required to create the database from the previous exercise using only SQL queries. Firstly, just create new database named Minions.
*/
CREATE DATABASE Minions


/* Problem 2 - Create Tables.
-- In the newly created database Minions add table Minions (Id, Name, Age). Then add new table Towns (Id, Name). Set Id columns of both tables to be primary key as constraint.
*/
USE Minions

CREATE TABLE Minions(Id INT PRIMARY KEY,
                     Name NVARCHAR(15),
                     Age INT)

CREATE TABLE Towns(Id INT PRIMARY KEY,
                   Name NVARCHAR(15))


/* Problem 3 - Alter Minions Table.
-- Change the structure of the Minions table to have new column TownId that would be of the same type as the Id column of Towns table. Add new constraint that makes TownId foreign key and references to Id column of Towns table.
*/
ALTER TABLE Minions
    ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)


/* Problem 4 - Insert Records in Both Tables.
-- Populate both tables with sample records given in the table below.
   |-------------------------------------|	
   | Minions                | Towns      |
   | Id	Name	Age  TownId | Id Name    |
   |-------------------------------------|
   | 1  Kevin   22   1	    | 1  Sofia	 |
   | 2  Bob     15   3      | 2  Plovdiv |
   | 3  Steward	NULL 2      | 3  Varna	 |
   |-------------------------------------|
   Use only SQL queries. Submit your INSERT statements as Run skeleton, run queries & check DB.
*/
INSERT INTO Towns (Id, Name)
    VALUES (1, 'Sofia'),
           (2, 'Plovdiv'),
           (3, 'Varna')
            
INSERT INTO Minions (Id, Name, Age, TownId)
    VALUES (1, 'Kevin', 22, 1),
           (2, 'Bob', 15, 3),
           (3, 'Steward', NULL, 2)


/* Problem 5 - Truncate Table Minions.
-- Delete all the data from the Minions table using SQL query.
*/
DELETE FROM Minions


/* Problem 6 - Drop All Tables.
-- Delete all tables from the Minions database using SQL query.
*/
DROP TABLE Minions, Towns


/* Problem 7 - Create Table People.
-- Using SQL query create table People with columns:
    • Id – unique number for every person there will be no more than 231-1 people. (Auto incremented)
    • Name – full name of the person will be no more than 200 Unicode characters. (Not null)
    • Picture – image with size up to 2 MB. (Allow nulls)
    • Height –  In meters. Real number precise up to 2 digits after floating point. (Allow nulls)
    • Weight –  In kilograms. Real number precise up to 2 digits after floating point. (Allow nulls)
    • Gender – Possible states are m or f. (Not null)
    • Birthdate – (Not null)
    • Biography – detailed biography of the person it can contain max allowed Unicode characters. (Allow nulls)
   Make Id primary key. Populate the table with only 5 records. Submit your CREATE and INSERT statements as Run queries & check DB.
*/
CREATE TABLE People (Id INT PRIMARY KEY IDENTITY,
                     Picture VARBINARY(MAX) CHECK (DATALENGTH(Picture) < 2048000),
                     Height DECIMAL(10, 2),
                     Weight DECIMAL(10, 2),
					 Gender CHAR CHECK (Gender IN('m', 'f')) NOT NULL,
					 Birthdate DATE NOT NULL,
					 Biography NVARCHAR(MAX))

INSERT INTO People (Picture, Height, Weight, Gender, Birthdate, Biography)
	VALUES (1024, 190, 14, 'f', '1991/04/14', 'Strange bio!'),
		   (2048, 1.90, 1.4, 'm', '1890/09/24', NULL),
		   (4096, 0.190, 0.14, 'f', '9999/01/09', 'Random biography!'),
		   (8192, 1900, 140, 'm', '0001/01/01', 'This is a bit longer biography'),
		   (16384, 19000, 1400, 'f', '1000/10/10', NULL)
