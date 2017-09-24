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
   | Id	Name	Age  TownId	| Id Name    |
   |-------------------------------------|
   | 1	Kevin   22   1	    | 1  Sofia	 |
   | 2	Bob	    15   3	    | 2  Plovdiv |
   | 3	Steward	NULL 2      | 3  Varna	 |
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


