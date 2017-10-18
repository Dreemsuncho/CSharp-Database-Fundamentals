/* Problem 1 - Ont-To-One Relationship.
-- Create two tables as follows. Use appropriate data types.
    |----------------------------------------------|-----------------------------|
    |                  Persons                     |         Passports           |
    |----------------------------------------------|-----------------------------|
    | PersonID | FirstName | Salary   | PassportID | PassportID | PassportNumber |
    | 1        | Roberto   | 43300.00 | 102        | 101        | N34FG21B       |
    | 2        | Tom       | 56100.00 | 103        | 102        | K65LO4R7       |
    | 3        | Yana      | 60200.00 | 101        | 103        | ZE657QP2       |
    |----------------------------------------------|-----------------------------|
   Insert the data from the example above.
   Alter the customers table and make PersonID a primary key. Create a foreign key between Persons and Passports by using PassportID column.
*/
CREATE TABLE Persons
(
	PersonID INT NOT NULL,
	FirstName VARCHAR(50),
	Salary MONEY,
	PassportID INT NOT NULL
)

CREATE TABLE Passports
(
	PassportID INT NOT NULL,
	PassportNumber VARCHAR(50)
)

INSERT INTO Persons 
(
	PersonID, 
	FirstName,
	Salary,
	PassportID
)
VALUES
	(1, 'Roberto', 43300.00, 102),
	(2, 'Tom', 56100.00, 103),
	(3, 'Yana', 60200.00, 101)
               
INSERT INTO Passports (PassportID, PassportNumber)
VALUES 
	(101, 'N34FG21B'),
	(102, 'K65LO4R7'),
	(103, 'ZE657QP2')

ALTER TABLE Passports
ADD PRIMARY KEY (PassportID)

ALTER TABLE Persons
ADD PRIMARY KEY (PersonID),
FOREIGN KEY (PassportID) REFERENCES Passports(PassportID)



/* Problem 2 - One-To-Many Relationship.
-- Create two tables as follows. Use appropriate data types.
    |------------------------------------|----------------------------------------|
    |              Models                |              Manufacturers             |
    |------------------------------------|----------------------------------------|
    | ModelID | Name    | ManufacturerID | ManufacturerID | Name  | EstablishedOn |
    | 101     | X1      | 1              | 1              | BMW   | 07/03/1916    |
    | 102     | i6      | 1              | 2              | Tesla | 01/01/2003    |
    | 103     | Model S | 2              | 3              | Lada  | 01/05/1966    |
    | 104     | Model X | 2              |----------------------------------------|
    | 105     | Model 3 | 2              |
    | 106     | Nova    | 3              |
    |------------------------------------|
   Insert the data from the example above. Add primary keys and foreign keys.
*/
CREATE TABLE Models
(
	ModelID INT NOT NULL,
	Name VARCHAR(50),
	ManufacturerID INT NOT NULL
)
					
CREATE TABLE Manufacturers
(
	ManufacturerID INT NOT NULL,
	Name VARCHAR(50),
	EstablishedON DATE
)

INSERT INTO Models
(
	ModelID, [Name], ManufacturerID
)
VALUES 
	(101, 'X1', 1),
	(102, 'i6', 1),
	(103, 'Model S', 2),
	(104, 'Model X', 2),	
	(105, 'Model 3', 2),	
	(106, 'Nova', 3)


INSERT INTO Manufacturers 
(
	ManufacturerID, [Name], EstablishedON
)
VALUES 
	(1, 'BMW', '07/03/1916'),
	(2, 'Tesla', '01/01/2003'),
	(3, 'Lada', '01/05/1966')

ALTER TABLE Manufacturers
ADD PRIMARY KEY (ManufacturerID)

ALTER TABLE Models
ADD PRIMARY KEY (ModelID),
FOREIGN KEY(ManufacturerID) REFERENCES Manufacturers(ManufacturerID)



/* Problem 3 - Many-To-Many Relationship.
-- Create three tables as follows. Use appropriate data types.
    |----------------------------------------|--------------------|
    |     Students     |         Exams       |     StudentsExams  |
    |----------------------------------------|--------------------|
    | StudentID | Name | ExamID | Name       | StudentID | ExamID |
    | 1         | Mila | 101    | SpringMVC  | 1         | 101    |
    | 2         | Toni | 102    | Neo4j      | 1         | 102    |
    | 3         | Ron  | 103    | Oracle 11g | 2         | 101    |
    |----------------------------------------| 3         | 103    |
                                             | 2         | 102    |
                                             | 2         | 103    |
                                             |--------------------|
   Insert the data from the example above.
   Add primary keys and foreign keys. Have in mind that table StudentsExams should have a composite primary key.
*/
CREATE TABLE Students 
(
	StudentID INT NOT NULL,
	[Name] VARCHAR(50)
)

CREATE TABLE Exams 
(
	ExamID INT NOT NULL,
	[Name] VARCHAR(50)
)

CREATE TABLE StudentsExams 
(
	StudentID INT NOT NULL,
	ExamID INT NOT NULL
)

ALTER TABLE Students
ADD PRIMARY KEY (StudentID)

ALTER TABLE Exams
ADD PRIMARY KEY (ExamID)

ALTER TABLE StudentsExams
ADD PRIMARY KEY (StudentID, ExamID),
FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
FOREIGN KEY (EXamID) REFERENCES Exams(ExamID)

INSERT INTO Students 
(
	StudentID, [Name]
)
VALUES 
	(1, 'Mila'),
	(2, 'Toni'),
	(3, 'Ron')

INSERT INTO Exams 
(
	ExamID, [Name]
)
VALUES 
	(101,'SpringMVC'),
	(102, 'Neo4j'),
	(103, 'Oracle 11g')

INSERT INTO StudentsExams (StudentID, ExamID)
VALUES 
	(1, 101),
	(1, 102),
	(2, 101),
	(3, 103),
	(2, 102),
	(2, 103)



/* Problem 4 - Self-Referencing.

-- Create a single table as follows. Use appropriate data types.
    |----------------------------|
    |         Teachers           |
    |----------------------------|
    | TeacherID | Name ManagerID |
    | 101       | John NULL      |
    | 102       | Maya 106       |
    | 103       | Silvia 106     |
    | 104       | Ted 105        |
    | 105       | Mark 101       |
    | 106       | Greta 101      |
    |----------------------------|
   Insert the data from the example above. Add primary keys and foreign keys. The foreign key should be 
   between ManagerId and TeacherId.
*/
CREATE TABLE Teachers
(
	TeacherID INT NOT NULL,
	[Name] VARCHAR(50),
	ManagerID INT
)

INSERT INTO Teachers 
(
	TeacherID, [Name], ManagerID
)
VALUES 
	(101, 'John', NULL),
	(102, 'Maya', 106),
	(103, 'Silvia', 106),
	(104, 'Ted', 105),
	(105, 'Mark', 101),
	(106, 'Greta', 101)

ALTER TABLE Teachers
ADD 
	PRIMARY KEY (TeacherID),
	FOREIGN KEY (ManagerID) REFERENCES Teachers(TeacherID)



/* Problem 5 - Online Store Database.

-- Create a new database and design the following structure:
   Hint: (See OnlineStore_Diagram.jpg in current repo)
*/
CREATE DATABASE OnlineStore
USE OnlineStore

CREATE TABLE ItemTypes 
(
	ItemTypeID INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50)
)

CREATE TABLE Items 
(
	ItemID INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50),
	ItemTypeID INT FOREIGN KEY REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE Cities
(
	CityID INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50)
)

CREATE TABLE Customers 
(
	CustomerID INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50),
	Birthday DATE,
	CityID INT FOREIGN KEY REFERENCES Cities(CityID)
)

CREATE TABLE Orders 
(
	OrderID INT PRIMARY KEY IDENTITY,
	CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID)
)

CREATE TABLE OrderItems
(
	OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
	ItemID INT FOREIGN KEY REFERENCES Items(ItemID), 
	PRIMARY KEY(OrderID, ItemID)
)



/* Problem 6 - University Database.

-- Create a new database and design the following structure:
   Hint: (See University_Database.jpg in current repo)
*/ 
CREATE DATABASE University
USE University

CREATE TABLE Subjects
(
	SubjectID INT PRIMARY KEY,
	SubjectName VARCHAR(50)
)

CREATE TABLE Majors
(
	MarjorID INT PRIMARY KEY,
	[Name] VARCHAR(50)
)

CREATE TABLE Students
(
	StudentID INT PRIMARY KEY,
	StudentNumber INT UNIQUE,
	StudentName VARCHAR(50),
	MarjorID INT FOREIGN KEY REFERENCES Majors(MarjorID)
)

CREATE TABLE Payments
(
	PaymentID INT PRIMARY KEY,
	PaymentDate DATE,
	PaymentAmount MONEY,
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID)
)

CREATE TABLE Agenda
(
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
	SubjectID INT FOREIGN KEY REFERENCES Subjects(SubjectID)
	PRIMARY KEY (StudentID, SubjectID)
)
-- In this problem one test doesn't pass for whatever reason :)



/* Problem 7 - * Peaks in Rila.
-- Display all peaks for "Rila" mountain. Include:
    • MountainRange
    • PeakName
    • PeakElevation
   Peaks should be sorted by elevation descending.
*/
USE [Geography]
GO
SELECT 
	MountainRange, 
	PeakName, 
	Elevation 
FROM 
	Peaks p
JOIN 
	Mountains m ON p.MountainId = m.Id
WHERE
	m.MountainRange = 'Rila'
ORDER BY
	Elevation DESC