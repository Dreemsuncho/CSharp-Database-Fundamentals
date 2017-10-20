-- PART I – Queries for Bank Database
USE Bank
GO
/* Problem 1 - Create Table Logs.

-- Create a table – Logs (LogId, AccountId, OldSum, NewSum). Add a trigger to the Accounts table 
   that enters a new entry into the Logs table every time the sum on an account changes.
*/
CREATE TABLE Logs
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT,
	OldSum MONEY,
	NewSum MONEY
)
GO

CREATE TRIGGER tr_SaveLogs
ON Accounts
AFTER UPDATE
AS BEGIN
	DECLARE @accountId INT = (SELECT Id FROM INSERTED)
	DECLARE @oldSum MONEY = (SELECT Balance FROM DELETED)
	DECLARE @newSum MONEY = (SELECT Balance FROM INSERTED)

	INSERT INTO 
		Logs (AccountId, OldSum, NewSum)
	VALUES
		(@accountId, @oldSum, @newSum)
END



/* Problem 2 - Create Table Emails.

-- Create another table – NotificationEmails(Id, Recipient, Subject, Body). 
   Add a trigger to logs table and create new email whenever new record is inserted in logs table. 
   The following data is required to be filled for each email:
    • Recipient – AccountId
    • Subject – “Balance change for account: {AccountId}”
    • Body - “On {date} your balance was changed from {old} to {new}.”
*/
CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT FOREIGN KEY REFERENCES Accounts(Id),
	[Subject] VARCHAR(100),
	Body VARCHAR(100)
)
GO

CREATE TRIGGER tr_CreateNofitication
ON Logs
AFTER INSERT
AS BEGIN
	DECLARE @recipient INT = (SELECT AccountId FROM INSERTED)
	DECLARE @subject VARCHAR(100) = CONCAT('Balance change for account: ', @recipient)
	DECLARE @oldSum MONEY = (SELECT OldSum FROM INSERTED)
	DECLARE @newSum MONEY = (SELECT NewSum FROM INSERTED)
	DECLARE @body VARCHAR(100) = CONCAT('On ', GETDATE(), ' your balance was changed from ', @oldSum, ' to ', @newSum, '.')

	INSERT INTO 
		NotificationEmails(Recipient, [Subject], Body)
	VALUES
		(@recipient, @subject, @body)
END



/* Problem 3 - Deposit Money.

-- Add stored procedure usp_DepositMoney (AccountId, MoneyAmount) that deposits money to an existing account. 
   Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. 
   The procedure should produce exact results working with the specified precision.
*/
GO
CREATE PROC usp_DepositMoney
	@AccountId INT,
	@MoneyAmount DECIMAL(10,4)
AS BEGIN
	IF @MoneyAmount > 0
	BEGIN
		UPDATE Accounts
		SET Balance += @MoneyAmount
		WHERE Id = @AccountId
	END
END



/* Problem 4 - Withdraw Money.

-- Add stored procedure usp_WithdrawMoney (AccountId, MoneyAmount) that withdraws money from an existing account. 
   Make sure to guarantee valid positive MoneyAmount with precision up to fourth sign after decimal point. 
   The procedure should produce exact results working with the specified precision.
*/
GO
CREATE PROC usp_WithdrawMoney
	@AccountId INT,
	@MoneyAmount DECIMAL(10,4)
AS BEGIN
	IF @MoneyAmount > 0
	BEGIN
		UPDATE Accounts
		SET Balance -= @MoneyAmount
		WHERE Id = @AccountId
	END
END



/* Problem 5 - Money Transfer.

-- Write stored procedure usp_TransferMoney(SenderId, ReceiverId, Amount) that transfers money from 
   one account to another. Make sure to guarantee valid positive MoneyAmount with precision up to 
   fourth sign after decimal point. Make sure that the whole procedure passes without errors and if 
   error occurs make no change in the database. You can use both: “usp_DepositMoney”, “usp_WithdrawMoney” 
   (look at previous two problems about those procedures).
*/
GO
CREATE PROC usp_TransferMoney 
	@SenderId INT,
	@RecieverId INT,
	@MoneyAmount DECIMAL(10,4)
AS BEGIN
	BEGIN TRANSACTION

	IF @MoneyAmount > 0
	BEGIN
		BEGIN TRY
			EXEC usp_WithdrawMoney @SenderId, @MoneyAmount
			EXEC usp_DepositMoney @RecieverId, @MoneyAmount
		END TRY
		BEGIN CATCH
			ROLLBACK
			RETURN
		END CATCH	
	END
	ELSE
	BEGIN 
		ROLLBACK
	END

	COMMIT
END
GO



-- PART II – Queries for Diablo Database
/*
   You are given a database "Diablo" holding users, games, items, characters and statistics available as SQL script. 
   Your task is to write some stored procedures, views and other server-side database objects and 
   write some SQL queries for displaying data from the database.
   Important: start with a clean copy of the "Diablo" database on each problem. Just execute the SQL script again.
*/
USE Diablo
GO
/* Problem 6 - Trigger.

-- 1. Users should not be allowed to buy items with higher level than their level. 
   Create a trigger that restricts that. The trigger should prevent inserting items that are 
   above specified level while allowing all others to be inserted.

-- 2. Add bonus cash of 50000 to users: baleremuda, loosenoise, inguinalself, 
   buildingdeltoid, monoxidecos in the game “Bali”.

-- 3. There are two groups of items that you must buy for the above users. The first are items with id 
   between 251 and 299 including. Second group are items with id between 501 and 539 including.
   Take off cash from each user for the bought items.

-- 4. Select all users in the current game (“Bali”) with their items. Display username, game name, 
   cash and item name. Sort the result by username alphabetically, then by item name alphabetically.
*/
-- 6.1
CREATE TRIGGER tr_CheckItemsForHigherLevel
ON UserGameItems
INSTEAD OF INSERT
AS BEGIN
	DECLARE @itemId INT = (SELECT ItemId FROM inserted)
	DECLARE @userGameId INT = (SELECT UserGameId FROM inserted)

	DECLARE @itemLevel INT = (SELECT MinLevel FROM Items WHERE Id = @itemId)
	DECLARE @userLevel INT = (SELECT [Level] FROM UsersGames WHERE GameId = @userGameId)

	IF @itemLevel <= @userLevel
	BEGIN
		INSERT INTO UserGameItems (ItemId, UserGameId)
		VALUES (@itemId, @userGameId)
	END
END

-- 6.2
UPDATE UsersGames
SET Cash += 50000
WHERE UserId IN
(
	SELECT UserId FROM Users WHERE Username IN
		(
			'baleremuda', 
			'loosenoise', 
			'inguinalself', 
			'buildingdeltoid', 
			'monoxidecos'
		)
) AND GameId = (SELECT Id FROM Games WHERE [Name] = 'Bali')

-- 6.3
WITH CTE_cte AS
(
	(
		SELECT Id 
		FROM UsersGames 
		WHERE UserId = 
			(
				SELECT Id 
				FROM Users 
				WHERE Username = 'baleremuda'
			) AND 
			GameId = 
			(
				SELECT Id 
				FROM Games 
				WHERE [Name] = 'Bali'
			)
	)
)
INSERT INTO	UserGameItems (ItemId, UserGameId)
VALUES
(
	(SELECT Id FROM Items WHERE (Id BETWEEN 251 AND 299) AND (Id BETWEEN 501 AND 539)),
	(SELECT * FROM CTE_cte)
)

DECLARE @itemsSum MONEY = (SELECT SUM(Price) FROM Items WHERE (Id BETWEEN 251 AND 299) AND (Id BETWEEN 501 AND 539))

UPDATE UsersGames
SET Cash -= @itemsSum
WHERE 
	GameId = (SELECT Id FROM Games WHERE [Name] = 'Bali') AND
	UserId IN (SELECT Id FROM Users WHERE UserName IN 
	(
		'baleremuda', 
		'loosenoise', 
		'inguinalself', 
		'buildingdeltoid', 
		'monoxidecos'
	))

-- 6.4
SELECT 
	u.UserName, 
	g.[Name], 
	ug.Cash, 
	i.[Name] AS [Item Name]
FROM
	Games g
JOIN
	UsersGames ug ON g.Id = ug.GameId
JOIN 
	Users u ON ug.UserId = u.Id
JOIN
	UserGameItems ugi ON ug.Id = ugi.UserGameId
JOIN
	Items i ON ugi.ItemId = i.Id
WHERE 
	g.[Name] = 'Bali'
ORDER BY
	u.Username,
	i.[Name]
GO



/* Problem 7 - * Massive Shopping.

-- User Stamat in Safflower game wants to buy some items. He likes all items from 
   Level 11 to 12 as well as all items from Level 19 to 21. As it is a bulk operation you have to use transactions. 
   A transaction is the operation of taking out the cash from the user in the current game as well as adding up the items. 
   Write transactions for each level range. If anything goes wrong turn back the changes inside of the transaction.
   Extract all of Stamat’s item names in the given game sorted by name alphabetically
*/
DECLARE @sumOfItemsFrom11To12Level MONEY = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 11 AND 12)
DECLARE @sumOfItemsFrom19To21Level MONEY = (SELECT SUM(Price) FROM Items WHERE MinLevel BETWEEN 19 AND 21)

DECLARE @userId INT = (SELECT Id FROM Users WHERE Username = 'Stamat')
DECLARE @gameId INT = (SELECT Id FROM Games WHERE [Name] = 'Safflower')
DECLARE @userGameId INT = (SELECT Id FROM UsersGames WHERE GameId = @gameId AND UserId = @userId)

BEGIN TRANSACTION
BEGIN TRY
	UPDATE UsersGames
	SET Cash -= @sumOfItemsFrom11To12Level
	WHERE Id = @userGameId
	
	DECLARE @userCash MONEY = (SELECT Cash FROM UsersGames WHERE Id = @userGameId)
	IF @userCash < 0
	BEGIN
		ROLLBACK
	END
	ELSE
	BEGIN
		INSERT INTO UserGameItems (ItemId, UserGameId)
		SELECT Id, @userGameId FROM Items WHERE  MinLevel BETWEEN 11 AND 12
		COMMIT
	END
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH

BEGIN TRANSACTION
BEGIN TRY
	UPDATE UsersGames
	SET Cash -= @sumOfItemsFrom19To21Level
	WHERE Id = @userGameId
	
	SET @userCash = (SELECT Cash FROM UsersGames WHERE Id = @userGameId)
	IF @userCash < 0
	BEGIN
		ROLLBACK
	END
	ELSE
	BEGIN
		INSERT INTO UserGameItems (ItemId, UserGameId)
		SELECT Id, @userGameId FROM Items WHERE  MinLevel BETWEEN 19 AND 21
		COMMIT
	END
END TRY
BEGIN CATCH
	ROLLBACK
END CATCH

SELECT
	i.[Name] AS [Item Name]
FROM
	Items i
JOIN
	UserGameItems ugi ON i.Id = ugi.ItemId
WHERE
	ugi.UserGameId = @userGameId
ORDER BY
	[Item Name]



-- Part III – Queries for SoftUni Database
USE SoftUni
GO
/* Problem 8 - Employees with Three Projects.

-- Create a procedure usp_AssignProject(@emloyeeId, @projectID) that assigns projects to employee. 
   If the employee has more than 3 project throw exception and rollback the changes. 
   The exception message must be: "The employee has too many projects!" with Severity = 16, State = 1.
*/
CREATE PROC usp_AssignProject
	@employeeId INT,
	@projectId INT
AS BEGIN
	BEGIN TRANSACTION
	DECLARE @userProjectsCount INT = (SELECT COUNT(*) FROM EmployeesProjects WHERE EmployeeID = @employeeId)
	IF @userProjectsCount >= 3
	BEGIN
		ROLLBACK
		RAISERROR('The employee has too many projects!', 16, 1)
		RETURN
	END
	ELSE
	BEGIN
		INSERT INTO EmployeesProjects (EmployeeID, ProjectID)
		VALUES (@employeeId, @projectId)
		COMMIT
	END
END



/* Problem 9 - Delete Employees.

-- Create a table Deleted_Employees(EmployeeId PK, FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary) 
   that will hold information about fired(deleted) employees from the Employees table. Add a trigger to Employees 
   table that inserts the corresponding information about the deleted records in Deleted_Employees.
*/
CREATE TABLE Deleted_Employees
(
	EmployeeId INT PRIMARY KEY IDENTITY, 
	FirstName VARCHAR(100), 
	LastName VARCHAR(100), 
	MiddleName VARCHAR(100), 
	JobTitle VARCHAR(100), 
	DepartmentId INT FOREIGN KEY REFERENCES Departments(DepartmentId), 
	Salary MONEY
)
GO
CREATE TRIGGER tr_DeleteEmployees
ON Employees
AFTER DELETE
AS BEGIN
	INSERT INTO Deleted_Employees 
	SELECT FirstName, LastName, MiddleName, JobTitle, DepartmentID, Salary
	FROM DELETED
END
