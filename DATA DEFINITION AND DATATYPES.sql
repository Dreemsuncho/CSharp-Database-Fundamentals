/* Problem 1 - Create Database.
-- You now know how to create database using the GUI of the SSMS. Now it�s time to create it using SQL queries. In that task (and the several following it) you will be required to create the database from the previous exercise using only SQL queries. Firstly, just create new database named Minions.
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
    � Id � unique number for every person there will be no more than 231-1 people. (Auto incremented)
    � Name � full name of the person will be no more than 200 Unicode characters. (Not null)
    � Picture � image with size up to 2 MB. (Allow nulls)
    � Height �  In meters. Real number precise up to 2 digits after floating point. (Allow nulls)
    � Weight �  In kilograms. Real number precise up to 2 digits after floating point. (Allow nulls)
    � Gender � Possible states are m or f. (Not null)
    � Birthdate � (Not null)
    � Biography � detailed biography of the person it can contain max allowed Unicode characters. (Allow nulls)
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


/* Problem 8 - Create Table Users.
-- Using SQL query create table Users with columns:
    � Id � unique number for every user. There will be no more than 263-1 users. (Auto incremented)
    � Username � unique identifier of the user will be no more than 30 characters (non Unicode). (Required)
    � Password � password will be no longer than 26 characters (non Unicode). (Required)
    � ProfilePicture � image with size up to 900 KB. 
    � LastLoginTime
    � IsDeleted � shows if the user deleted his/her profile. Possible states are true or false.
   Make Id primary key. Populate the table with exactly 5 records. Submit your CREATE and INSERT statements as Run queries & check DB.
*/
CREATE TABLE Users (Id BIGINT CONSTRAINT PK_Users PRIMARY KEY IDENTITY CHECK (Id <	9223372036854775808 - 1),
                    Username VARCHAR(30) UNIQUE NOT NULL,
                    Password VARCHAR(26) NOT NULL,
                    ProfilePicture VARBINARY CHECK (DATALENGTH(ProfilePicture) <= 900),
                    LastLoginTime DATETIME,
                    IsDeleted BIT NOT NULL)
					
INSERT INTO Users (Username, Password, ProfilePicture, LastLoginTime, IsDeleted)
    VALUES ('Elvis', '123321', NULL, '2017/09/21 22:43:14', 0),
           ('Go6o', 'Pass', 899, '2017/09/21 21:43:14', 1),
           ('Kiro', 'Pass123', 0, '2007/02/11 21:43:14', 0),
           ('Gri6o', 'dasdas123asdasd', 900, '2011/11/11 01:23:14', 1),
           ('Ti6o', '123das123', 900, '2021/12/21 02:33:44', 1)


/* Problem 9 - Change Primary Key.
-- Using SQL queries modify table Users from the previous task. First remove current primary key then create new primary key that would be the combination of fields Id and Username.
*/
ALTER TABLE Users
	DROP CONSTRAINT PK_Users
	
ALTER TABLE Users
	ADD CONSTRAINT PK_Users PRIMARY KEY (Id, Username)


/* Problem 10 - Add Check Constraint.
-- Using SQL queries modify table Users. Add check constraint to ensure that the values in the Password field are at least 5 symbols long. 
*/
UPDATE Users
	SET Password = '01234'
	WHERE (LEN(Password) < 5)

ALTER TABLE Users
	ADD CONSTRAINT CK__Password__Min CHECK (LEN(Password) >= 5)


/* Problem 11 - Set Default Value of a Field.
-- Using SQL queries modify table Users. Make the default value of LastLoginTime field to be the current time.
*/
ALTER TABLE Users
	ADD  DEFAULT(GETDATE()) FOR LastLoginTime


/* Problem 12 - Set Unique Field.
-- Using SQL queries modify table Users. Remove Username field from the primary key so only the field Id would be primary key. Now add unique constraint to the Username field to ensure that the values there are at least 3 symbols long.
*/
ALTER TABLE Users
	DROP CONSTRAINT PK_Users

ALTER TABLE Users
	ADD CONSTRAINT PK_Users PRIMARY KEY (Id),
		CONSTRAINT UC_Username UNIQUE (Username),
		CHECK (LEN(Username) >= 3)


/* Problem 13 - Movies Database.
-- Using SQL queries create Movies database with the following entities:
    � Directors (Id, DirectorName, Notes)
    � Genres (Id, GenreName, Notes)
    � Categories (Id, CategoryName, Notes)
    � Movies (Id, Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
    Set most appropriate data types for each column. Set primary key to each table. Populate each table with exactly 5 records. Make sure the columns that are present in 2 tables would be of the same data type. Consider which fields are always required and which are optional. Submit your CREATE TABLE and INSERT statements as Run queries & check DB.
*/
CREATE DATABASE Movies

USE Movies 
GO

CREATE TABLE Directors (Id INT PRIMARY KEY IDENTITY,
                        DirectorName VARCHAR(55) NOT NULL, 
                        Notes TEXT)

CREATE TABLE Genres (Id INT PRIMARY KEY IDENTITY,
                     GenreName VARCHAR(55) NOT NULL,
                     Notes TEXT)

CREATE TABLE Categories (Id INT PRIMARY KEY IDENTITY,
                         CategoryName VARCHAR(55) NOT NULL,
                         Notes TEXT)

CREATE TABLE Movies (Id INT PRIMARY KEY IDENTITY,
                     Title VARCHAR(55) NOT NULL,
                     DirectorId INT FOREIGN KEY (DirectorId) REFERENCES Directors(Id) NOT NULL,
                     CopyrightYear DATE,
                     Length TIME,
                     GenreId INT FOREIGN KEY (GenreId) REFERENCES Genres(Id) NOT NULL,
                     CategoryId INT FOREIGN KEY (CategoryId) REFERENCES Categories(Id) NOT NULL,
                     Rating INT,
                     Notes TEXT)

INSERT INTO Directors (DirectorName, Notes)
	VALUES ('Dir name', 'First note'), 
           ('Another director', NULL),
           ('Third director', 'Random note'),
           ('Fourth director', NULL),
           ('Fifth director', NULL)

INSERT INTO Genres (GenreName, Notes)
	VALUES ('First genre name', 'First note'),
           ('Second genre name', 'Second note'),
           ('Third genre name', 'Third note'),
           ('Fourth genre name', 'Fourth note'),
           ('Fifth genre name', 'Fifth note')

INSERT INTO Categories (CategoryName, Notes)
	VALUES ('First category name', 'First note'),
           ('Second category name', 'Second note'),
           ('Third category name', 'Third note'),
           ('Fourth category name', 'Fourth note'),
           ('Fifth category name', 'Fifth note')

INSERT INTO Movies (Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes)
	VALUES ('First title', 1, GETDATE(), '11:24:44', 1, 1, 10, 'First note'),
           ('Second title', 2, GETDATE(), '12:24:44', 2, 2, 9, 'Second note'),
           ('Third title', 3, GETDATE(), '13:24:44', 3, 3, 8, 'Third note'),
           ('Fourth title', 4, GETDATE(), '14:24:44', 4, 4, 7, NULL),
           ('Fifth title', 5, GETDATE(), '15:24:44', 5, 5, 6, 'Fourth note')