/* Problem 1 - Employees with Salary Above 35000.

-- Create stored procedure usp_GetEmployeesSalaryAbove35000 that returns all employees’ 
   first and last names for whose salary is above 35000. 
*/
USE SoftUni
GO
CREATE PROC usp_GetEmployeesSalaryAbove35000
AS
BEGIN
	SELECT
		FirstName,
		LastName 
	FROM 
		Employees
	WHERE 
		Salary > 35000
END
GO



/* Problem 2 - Employees with Salary Above Number.

-- Create stored procedure usp_GetEmployeesSalaryAboveNumber that accept a number (of type DECIMAL(18,4)) 
   as parameter and return all employees’ first and last names whose salary is above or equal to the given number. 
*/
CREATE PROC usp_GetEmployeesSalaryAboveNumber @number DECIMAL(18, 4)
AS
BEGIN
	SELECT 
		FirstName,
		LastName
	FROM 
		Employees
	WHERE 
		Salary >= @number
END
GO



/* Problem 3 - Town Names Starting With.

-- Write a stored procedure usp_GetTownsStartingWith that accept string as parameter and returns all 
   town names starting with that string. 
*/
CREATE PROC usp_GetTownsStartingWith @startText TEXT
AS
BEGIN
	SELECT
		[Name] AS Town
	FROM
		Towns
	WHERE
		[Name] LIKE CONCAT(@startText, '%')
END
GO



/* Problem 4 - Employees From Town.

-- Write a stored procedure usp_GetEmployeesFromTown that accepts town name as parameter and return the employees’ 
   first and last name that live in the given town. 
*/
CREATE PROC usp_GetEmployeesFromTown @townName VARCHAR(50)
AS
BEGIN
	SELECT 
		e.FirstName AS [First Name], 
		e.LastName AS [Last Name]
	FROM Employees e
	JOIN 
		Addresses a ON e.AddressID = a.AddressID
	JOIN 
		Towns t ON a.TownID = t.TownID
	WHERE 
		t.[Name] = @townName
END
GO



/* Problem 5 - Salary Level Function.

-- Write a function ufn_GetSalaryLevel(@salary DECIMAL(18,4)) that receives salary of an 
   employee and returns the level of the salary.
    • If salary is < 30000 return “Low”
    • If salary is between 30000 and 50000 (inclusive) return “Average”
    • If salary is > 50000 return “High”
*/
USE Bank
GO
CREATE FUNCTION ufn_GetSalaryLevel(@salary MONEY)
RETURNS VARCHAR(7)
AS
BEGIN
	DECLARE @result VARCHAR(7)

	IF @salary < 30000
		SET @result = 'Low'
	ELSE IF @salary BETWEEN 30000 AND 50000
		SET @result = 'Average'
	ELSE
		SET @result = 'High' 

	RETURN @result
END
GO



/* Problem 6 - Employees by Salary Level.

-- Write a stored procedure usp_EmployeesBySalaryLevel that receive as parameter level of salary (low, average or high) and 
   print the names of all employees that have given level of salary. 
   You should use the function - “dbo.ufn_GetSalaryLevel(@Salary)”, which was part of the previous task, 
   inside your “CREATE PROCEDURE …” query.
*/
CREATE PROC usp_EmployeesBySalaryLevel @level VARCHAR(7)
AS
BEGIN
	SELECT 
		FirstName AS [First Name],
		LastName AS [Last Name]
	FROM
		 Employees
	WHERE 
		dbo.ufn_GetSalaryLevel(Salary) = @level
END
GO



/* Problem 7 - Define Function.

-- Define a function ufn_IsWordComprised(@setOfLetters, @word) that returns true or false depending on 
   that if the word is a comprised of the given set of letters. 
*/
CREATE FUNCTION ufn_IsWordComprised
(
	@setOfLetters VARCHAR(50), 
	@word VARCHAR(50)
)
RETURNS BIT
AS
BEGIN
	DECLARE @result BIT = 1
	DECLARE @i INT = 1

	WHILE @i < LEN(@word) + 1
	BEGIN
		DECLARE @letter CHAR = LOWER(SUBSTRING(@word, @i, 1))
		DECLARE @charIndex INT = CHARINDEX(@letter, LOWER(@setOfLetters));

		IF @charIndex < 1
		BEGIN 
			SET @result = 0
			BREAK;
		END

		SET @i += 1
	END

	RETURN @result
END
GO



/* Problem 8 - * Delete Employees and Departments.

-- Write a procedure with the name usp_DeleteEmployeesFromDepartment (@departmentId INT) which deletes all Employees 
   from a given department. Delete these departments from the Departments table too. 
   Finally SELECT the number of employees from the given department. If the delete statements are correct the select query should return 0.
*/
CREATE FUNCTION ufn_EmployeesIDsByDepartment (@id INT)
RETURNS @result TABLE
(
	EmployeeID INT
)
AS BEGIN
	INSERT INTO @result (EmployeeID)
	SELECT EmployeeID 
	FROM Employees 
	WHERE DepartmentID = @id

	RETURN 
END
GO

CREATE PROC usp_DeleteEmployeesFromDepartment @departmentId INT
AS
BEGIN

	DELETE EmployeesProjects
	WHERE EmployeeID IN (SELECT * FROM dbo.ufn_EmployeesIDsByDepartment (@departmentId))

	UPDATE EmployeesProjects
	SET EmployeeID = NULL
	WHERE EmployeeId IN (SELECT * FROM dbo.ufn_EmployeesIDsByDepartment (@departmentId))

	ALTER TABLE Departments
	ALTER COLUMN ManagerId INT NULL

	UPDATE Departments
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT * FROM dbo.ufn_EmployeesIDsByDepartment (@departmentId))
  
	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerId IN (SELECT * FROM dbo.ufn_EmployeesIDsByDepartment (@departmentId))

	DELETE Employees
	WHERE Employees.EmployeeID  IN (SELECT * FROM dbo.ufn_EmployeesIDsByDepartment (@departmentId))

	DELETE FROM  Employees
	WHERE DepartmentID = @departmentId

	DELETE FROM  Departments
	WHERE DepartmentID = @departmentId

	SELECT COUNT(*) 
	FROM Departments 
	WHERE DepartmentID = @departmentId
END
GO
DECLARE @departmentId INT = 1
EXEC usp_DeleteEmployeesFromDepartment @departmentId
	


-- PART II – Queries for Bank Database
USE Bank
GO
/* Problem 9 - Find Full Name.

-- You are given a database schema with tables AccountHolders(Id (PK), FirstName, LastName, SSN) and Accounts(Id (PK), 
   AccountHolderId (FK), Balance).  Write a stored procedure usp_GetHoldersFullName that selects the full names of all people.
*/
CREATE PROC usp_GetHoldersFullName 
AS
BEGIN
	SELECT CONCAT(FirstName, ' ', LastName) AS [Full Name] FROM AccountHolders
END
GO



/* Problem 10 - People with Balance Higher than.
-- Your task is to create a stored procedure usp_GetHoldersWithBalanceHigherThan that accepts a number as a 
   parameter and returns all people who have more money in total of all their accounts than the supplied number.
*/
CREATE PROC usp_GetHoldersWithBalanceHigherThan @amount MONEY
AS
BEGIN
	SELECT 
		FirstName AS [First Name],
		LastName AS [Last Name]
	FROM 
	(
		SELECT
			ah.FirstName, 
			ah.LastName, 
			SUM(a.Balance) AS Sum 
		FROM
			Accounts a
		JOIN 
			AccountHolders ah ON a.AccountHolderId = ah.Id
		GROUP BY 
			ah.FirstName,
			ah.LastName
		HAVING
			SUM(a.Balance) > @amount
	) AS AccWithBalance
END
GO



/* Problem 11 - Futire Value Function.

-- Your task is to create a function ufn_CalculateFutureValue that accepts as parameters – sum (money), yearly interest rate (float) 
   and number of years(int). It should calculate and return the future value of the initial sum. Using the following formula:
   FV = I × ((1 + R) ^ T)
    I – Initial sum
    R – Yearly interest rate
    T – Number of years
*/
CREATE FUNCTION dbo.ufn_CalculateFutureValue
(
	@money MONEY,
	@yearInterestRate FLOAT,
	@years INT
)
RETURNS MONEY
AS
BEGIN
	RETURN @money * POWER((1 + @yearInterestRate), @years)
END
GO



/* Problem 12 - Calculatin Interest.

-- Your task is to create a stored procedure usp_CalculateFutureValueForAccount that uses the function from the previous 
   problem to give an interest to a person's account for 5 years, along with information about his/her account id, 
   first name, last name and current balance as it is shown in the example below. 
   It should take the AccountId and the interest rate as parameters. Again you are provided with “dbo.ufn_CalculateFutureValue” 
   function which was part of the previous task.
*/
CREATE PROC usp_CalculateFutureValueForAccount @accountId INT, @interestRate FLOAT
AS
BEGIN
	SELECT 
		a.Id AS [Account Id],
		ah.FirstName AS [First Name],
		ah.LastName AS [Last Name],
		a.Balance AS [Current Balanace],
		dbo.ufn_CalculateFutureValue(a.Balance, @interestRate, 5) AS [Balance in 5 years]
	FROM
		AccountHolders ah
	JOIN
		Accounts a ON ah.Id = a.AccountHolderId
	WHERE
		a.Id = @accountId
END
GO



-- PART III – Queries for Diablo Database.
USE Diablo
GO
/*
   You are given a database "Diablo" holding users, games, items, characters and statistics available as SQL script. 
   Your task is to write some stored procedures, views and other server-side database objects and write some SQL queries 
   for displaying data from the database.
   Important: start with a clean copy of the "Diablo" database on each problem. Just execute the SQL script again. 
*/

/* Problem 13 - * Scalar Function: Cash in User Games Odd Rows.

-- Create a function ufn_CashInUsersGames that sums the cash of odd rows.Rows must be ordered by cash in descending order. 
   The function should take a game name as a parameter and return the result as table.
   Execute the function over the following game names, ordered exactly like: “Lily Stargazer”, “Love in a mist”.
*/

CREATE FUNCTION ufn_CashInUsersGames(@game NVARCHAR(40))
RETURNS @result TABLE (SumCash MONEY)
AS
BEGIN
	INSERT INTO @result (SumCash)
	SELECT SUM(Cash) AS SumCash
	FROM 
	(
		SELECT
			Cash, ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RowNumber
		FROM
			UsersGames ug
		JOIN 
			Games g ON ug.GameId = g.Id
		WHERE
			g.Name = @game
	) AS g
	WHERE
		RowNumber % 2 <> 0

	RETURN
END
