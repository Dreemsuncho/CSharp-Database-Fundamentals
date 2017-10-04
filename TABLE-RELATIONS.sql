/* Problem 1 - Ont-To-One Relationship.
-- Create two tables as follows. Use appropriate data types.
   |----------------------------------------------|-----------------------------|
   |                  Persons                     |         Passports           |
   |----------------------------------------------|-----------------------------|
   | PersonID | FirstName | Salary   | PassportID | PassportID | PassportNumber |
   | 1        | Roberto   | 43300.00 | 102        | 101        | N34FG21B	    |
   | 2        | Tom       | 56100.00 | 103        | 102        | K65LO4R7       |
   | 3        | Yana      | 60200.00 | 101        | 103        | ZE657QP2       |
   |----------------------------------------------|-----------------------------|
   Insert the data from the example above.
   Alter the customers table and make PersonID a primary key. Create a foreign key between Persons and Passports by using PassportID column.
*/
CREATE TABLE Persons(PersonID INT NOT NULL,
                     FirstName VARCHAR(50),
                     Salary MONEY,
                     PassportID INT NOT NULL)

CREATE TABLE Passports(PassportID INT NOT NULL,
                       PassportNumber VARCHAR(50))

INSERT INTO Persons (PersonID, FirstName,Salary,PassportID)
    VALUES (1, 'Roberto', 43300.00, 102),
           (2, 'Tom', 56100.00, 103),
           (3, 'Yana', 60200.00, 101)
               
INSERT INTO Passports (PassportID, PassportNumber)
    VALUES (101, 'N34FG21B'),
           (102, 'K65LO4R7'),
           (103, 'ZE657QP2')

ALTER TABLE Passports
	ADD PRIMARY KEY (PassportID)

ALTER TABLE Persons
	ADD PRIMARY KEY (PersonID),
        FOREIGN KEY (PassportID) REFERENCES Passports(PassportID)
