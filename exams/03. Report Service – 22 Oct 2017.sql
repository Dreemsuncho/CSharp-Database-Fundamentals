-- Section 1. DDL

/* Problem 1 - Database Design.

-- Crеate a database called ReportService. You need to create 6 tables:
    ● Users – contains information about the people who submit reports
    ● Reports - contains information about the submitted problems
    ● Employees – contains information about the people employees who work on reports
    ● Departments – contains information about the departments 
    ● Categories – contains information about categories inside the departments.
    ● Status - contains information about the possible statuses of a report
*/

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY,
	Username NVARCHAR(30) UNIQUE NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(50),
	Gender CHAR CHECK (Gender = 'M' OR Gender = 'F'),
	BirthDate DATETIME,
	Age INT,
	Email NVARCHAR(50) NOT NULL
)

CREATE TABLE Departments 
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR CHECK(Gender = 'M' OR Gender = 'F'),
	BirthDate DATETIME,
	Age INT,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Categories 
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE [Status]
(
	Id INT PRIMARY KEY IDENTITY,
	Label VARCHAR(30) NOT NULL
)

CREATE TABLE Reports
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	StatusId INT FOREIGN KEY REFERENCES [Status](Id) NOT NULL,
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME,
	[Description] VARCHAR(200),
	UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)



-- Section 2. DML (10 pts)
/*
   Before you start you have to import “DataSet-ReportService.sql”. If you have created the structure correctly 
   the data should be successfully inserted.
   In this section, you have to do some data manipulations:
*/

/* Problem 2 - Insert.

-- Let’s insert some sample data into the database. Write a query to add the following records into 
   the corresponding tables. All Id’s should be auto-generated. Replace names that relate to other tables with 
   the appropriate ID (look them up manually, there is no need to perform table joins).
*/
GO
CREATE FUNCTION udf_GetDepartmentId(@departmentName NVARCHAR(50))
RETURNS INT
AS BEGIN
	DECLARE @result INT = (SELECT Id FROM Departments WHERE [Name] = @departmentName)
	RETURN @result
END
GO 
CREATE FUNCTION udf_GetCategorytId(@categoryName NVARCHAR(50))
RETURNS INT
AS BEGIN
	DECLARE @result INT = (SELECT Id FROM Categories WHERE [Name] = @categoryName)
	RETURN @result
END
GO
CREATE FUNCTION udf_GetStatusId(@statusLabel NVARCHAR(50))
RETURNS INT
AS BEGIN
	DECLARE @result INT = (SELECT Id FROM [Status] WHERE Label = @statusLabel)
	RETURN @result
END
GO

INSERT INTO Employees 
(
	FirstName,
	LastName,
	Gender,
	Birthdate,
	DepartmentId
)
VALUES 
	('Marlo','O’Malley','M','9/21/1958', dbo.udf_GetDepartmentId ('Infrastructure')),
	('Niki','Stanaghan','F','11/26/1969', dbo.udf_GetDepartmentId ('Emergency')),
	('Ayrton','Senna','M','03/21/1960', dbo.udf_GetDepartmentId ('Event Management')),
	('Ronnie','Peterson','M','02/14/1944', dbo.udf_GetDepartmentId ('Event Management')),
	('Giovanna','Amati','F','07/20/1959', dbo.udf_GetDepartmentId ('Roads Maintenance'))

INSERT INTO Reports
(
	CategoryId,
	StatusId,
	OpenDate,
	CloseDate,
	[Description],
	UserId,
	EmployeeId
)
VALUES
	(
		(SELECT dbo.udf_GetCategorytId('Snow Removal')),
		(SELECT dbo.udf_GetStatusId('waiting')), 
		'04/13/2017',NULL, 'Stuck Road on Str.133',	6,2
	),
	(
		(SELECT dbo.udf_GetCategorytId('Sports Events')),
		(SELECT dbo.udf_GetStatusId('completed')), 
		'09/05/2015','12/06/2015','Charity trail running',3,5
	),
	(
		(SELECT dbo.udf_GetCategorytId('Dangerous Building')),
		(SELECT dbo.udf_GetStatusId('in progress')), 
		'09/07/2015',NULL,'Falling bricks on Str.58',5,2
	),
	(
		(SELECT dbo.udf_GetCategorytId('Streetlight')),
		(SELECT dbo.udf_GetStatusId('Streetlight')), 
		'07/03/2017','07/06/2017','Cut off streetlight on Str.11',1,1
	)



/* Problem 3 - Update.

-- Switch all report’s status to “in progress” where it is currently “waiting” for the “Streetlight” 
   category (look up the category ID and status ID’s manually, there is no need to use table joins).
*/
UPDATE 
	[Status]
SET 
	Label = 'in progress'
FROM 
	Reports r 
JOIN 
	Categories c ON c.Id = r.CategoryId
WHERE 
	[Status].Id = r.StatusId AND 
	[Status].Label = 'waiting' AND 
	c.[Name] = 'Streetlight'



/* Problem 4 - Delete.

-- Delete all reports who have a status “blocked”.
*/
DELETE FROM Reports
WHERE StatusId IN (SELECT ID FROM [Status] WHERE Label = 'blocked')



-- Section 3. Querying (40 pts)

/*
   You need to start with a fresh dataset, so recreate your DB and import the sample data 
   again (DataSet_ReportService.sql). 
   If not specified the ordering will be ascending.
*/

/* Problem 5 - Users by Age

-- Select all Usernames with their age ordered by age (ascending) then by username (descending). 
   Required columns:
    ● Username
    ● Age
*/
SELECT Username, Age
FROM Users
ORDER BY Age, Username DESC



/* Problem 6 - Unassigned Reports.

-- Find all reports that don’t have an assigned employee. Order the results by open date in ascending order, 
   then by description (ascending).
   Required columns:
    ● Description
    ● OpenDate
*/
SELECT [Description], OpenDate
FROM Reports
WHERE EmployeeId IS NULL
ORDER BY OpenDate, [Description]



/* Problem 7 - Employees & Reports.

-- Select only employees who have an assigned report and show all reports of each found employee. 
   Show the open date column in the format “yyyy-MM-dd”. Order them by employee id (ascending) then 
   by open date (ascending) and then by report Id (again ascending).
    Required columns:
    ● FirstName
    ● LastName
    ● Description
    ● OpenDate
*/
SELECT 
	e.FirstName,
	e.LastName,
	r.[Description],
	FORMAT(r.OpenDate,'yyyy-MM-dd') AS OpenDate
FROM 
	Employees e
JOIN 
	Reports r ON r.EmployeeId = e.Id 
ORDER BY
	r.EmployeeId,
	r.OpenDate,
	r.Id



/* Problem 8 - Most reported Category

-- Select ALL categories and order them by the number of reports per category in descending order and then alphabetically by name.
   Required columns:
    ● CategoryName
    ● ReportsNumber
*/
SELECT 
	c.[Name], 
	COUNT(r.Id) AS ReportsNumber
FROM 
	Categories c
LEFT JOIN 
	Reports r ON c.Id = r.CategoryId
GROUP BY 
	c.[Name]
ORDER BY
	ReportsNumber DESC,
	c.[Name]



/* Problem 9 - Employees in Category.

-- Select ALL categories and the number of employees in each category and order them alphabetically by category name.
   Required columns:
    ● CategoryName
    ● Employees Number
*/
SELECT 
	c.[Name] AS CategoryName,
	COUNT(e.Id) AS [Employees Number]
FROM 
	Categories c
JOIN 
	Departments d ON c.DepartmentId = d.Id
JOIN 
	Employees e ON d.Id = e.DepartmentId
GROUP BY
	c.[Name]



/* Problem 10 - Users per Employee.

-- Select all employees and show how many unique users each of them have served to.
   Required columns:
    ● Employee’s name - Full name consisting of FirstName and LastName and a space between them 
    ● User’s number
   Order by Users Number descending and then by Name ascending.
*/
SELECT 
	CONCAT(e.FirstName, ' ', e.LastName) AS [Name],
	COUNT(DISTINCT r.UserId) [Users Number]
FROM 
	Employees e
LEFT JOIN 
	Reports r ON e.Id = r.EmployeeId
GROUP BY
	CONCAT(e.FirstName, ' ', e.LastName) 
ORDER BY
	[Users Number] DESC,
	[Name]


	
/* Problem 11 - Emergency Patrol.

-- Select all reports which satisfy all the following criteria:
    ● are not closed yet (they don’t have a CloseDate)
    ● the description is longer than 20 symbols and the word “str” is mentioned anywhere
    ● are assigned to one of the following departments: “Infrastructure”, “Emergency”, “Roads Maintenance”

   Order the results by OpenDate (ascending), then by Reporter’s Email (ascending) and then by Report Id (ascending).
   Required columns:
    ● OpenDate
    ● Description
    ● Reporter Email
*/
SELECT 
	r.OpenDate,
	r.[Description],
	u.Email
FROM
	Reports r
JOIN
	Categories c ON r.CategoryId = c.Id
JOIN
	Departments d ON d.Id = c.DepartmentId
JOIN
	Users u ON u.Id = r.UserId
WHERE
	r.CloseDate IS NULL AND
	LEN(r.[Description]) > 20 AND
	r.[Description] LIKE '%str%' AND
	d.Id IN 
	(
		SELECT Id 
		FROM Departments 
		WHERE [Name] IN ('Infrastructure', 'Emergency', 'Roads Maintenance')
	)
ORDER BY
	r.OpenDate,
	u.Email,
	r.Id



/* Problem 12 - Birthday Report

-- Select all categories in which users have submitted a report on their birthday. Order them by name alphabetically.
   Required columns:
    ● Category Name
*/
SELECT 
	c.[Name] AS [Category Name]
FROM 
	Users u
JOIN 
	Reports r ON r.UserId = u.Id
JOIN 
	Categories c ON c.Id = r.CategoryId
WHERE 
	u.BirthDate = r.OpenDate
ORDER BY 
	c.[Name]



/* Problem 13 - Numbers Coincidence

-- Select all unique usernames which:
    ● starts with a digit and have reported in a category with id equal to the digit
    OR
    ● ends with a digit and have reported in a category with id equal to the digit

   Required columns:
    ● Username
   Order them alphabetically.
*/
SELECT Username
FROM Users u
JOIN Reports r ON u.Id = r.UserId
JOIN Categories c ON r.CategoryId = c.Id
WHERE 
	Username LIKE '[0-9]%' AND CAST(LEFT(Username, 1) AS INT) = c.Id OR
	Username LIKE '%[0-9]' AND CAST(RIGHT(Username, 1) AS INT) = c.Id
ORDER BY
	Username



/* Problem 14 - Average Closing Time

-- Select all departments that have been reported in and the average time for closing a report for each department 
   rounded to the closest integer part. If there is no information (e.g. none closed reports) about 
   any department fill in the Average Duration column “no info”.
   Required columns:
    ● Department Name 
    ● Average Duration - in days
   Order them by department name.
*/
SELECT 
	d.[Name],
	ISNULL(CAST(AVG(DATEDIFF(DAY,r.OpenDate,r.CloseDate)) AS VARCHAR), 'no info')
FROM 
	Departments d
JOIN
	Categories c ON c.DepartmentId = d.Id
JOIN 
	Reports r ON c.Id = r.CategoryId
GROUP BY
	d.[Name]
ORDER BY
	d.[Name]



/* Problem 15 - Favorite Categories

-- Select all departments with their categories where users have submitted a report. Show the distribution 
   of reports among the categories of each department in percentages without decimal part.
   Required columns:
    ● Department Name 
    ● Category Name
    ● Percentage
   Order them by department name, then by category name and then by percentage (all in ascending order).
*/

WITH CTE 
AS 
(
	SELECT 
		COUNT(r.Id) AS [Count],
		d.Id
	FROM 
		Departments d
	JOIN 
		Categories c ON d.Id = c.DepartmentId
	JOIN 
		Reports r ON r.CategoryId = c.Id
	GROUP BY
		d.[Name],d.Id
)
SELECT 
	d.[Name] AS [Department Name],
	c.[Name] AS [Category Name],
	CAST((CAST(COUNT(r.Id) AS DECIMAL(10,4))/ CTE.Count) * 100 AS INT) AS [Percentage]
FROM 
	Departments d
JOIN 
	Categories c ON d.Id = c.DepartmentId
JOIN 
	Reports r ON r.CategoryId = c.Id
JOIN
	CTE ON cte.Id = d.Id
GROUP BY
	d.[Name],c.[Name],CTE.Count
ORDER BY
	d.[Name],
	c.[Name],
	[Percentage]



-- Section 4. Programmability (14 pts)

/* Problem 16 - Employee's Load

-- Create a user defined function with the name udf_GetReportsCount(@employeeId, @statusId) that receives an 
   employee’s Id and a status Id returns the sum of the reports he is assigned to with the given status.
   Parameters:
    ● Employee’s Id
    ● Status Id
*/
GO
CREATE FUNCTION udf_GetReportsCount(@employeeId INT, @statusId INT) 
RETURNS INT
AS BEGIN
	DECLARE @result INT = 
	(
		SELECT COUNT(*) 
		FROM Reports r
		WHERE r.EmployeeId = @employeeId AND r.StatusId = @statusId
	)

	RETURN @result
END



/* Problem 17 - Assign Employee

-- Create a user defined stored procedure with the name usp_AssignEmployeeToReport(@employeeId, @reportId) 
   that receives an employee’s Id and a report’s Id and assigns the employee to the report only 
   if the department of the employee and the department of the report’s category are the same. 
   If the assigning is not successful rollback any changes and throw an exception with message: 
   “Employee doesn't belong to the appropriate department!”. 
   Parameters:
    ● Employee’s Id
    ● Report’s Id
*/
GO
CREATE PROC usp_AssignEmployeeToReport 
	@employeeId INT, 
	@reportId INT
AS BEGIN
	DECLARE @departmentEmployeeId INT = 
	(
		SELECT e.DepartmentId
		FROM Employees e
		WHERE e.Id = @employeeId
	)

	DECLARE @departmentCategoryReportId INT = 
	(
		SELECT c.DepartmentId
		FROM Categories c
		JOIN Reports r ON c.Id = r.CategoryId
		WHERE r.Id = @reportId
	)

	IF @departmentEmployeeId = @departmentCategoryReportId
	BEGIN 
		UPDATE Reports
		SET EmployeeId = @employeeId
		WHERE Id = @reportId
	END
	ELSE
	BEGIN
		RAISERROR('Employee doesn''t belong to the appropriate department!', 16, 1)
		RETURN
	END
END

EXEC usp_AssignEmployeeToReport 17, 2;
SELECT EmployeeId FROM Reports WHERE id = 2



/* Problem 18 - Close Reports

-- Create a trigger which changes the StatusId to “completed” of each report after a CloseDate 
   is entered for the report. 
*/
GO
CREATE TRIGGER tr_changeStatus
ON Reports
AFTER UPDATE
AS BEGIN
	declare @reportId INT = 
	(
		SELECT d.Id 
			FROM DELETED d 
		JOIN 
			inserted i ON d.Id = i.Id
		WHERE
			i.CloseDate <> d.CloseDate
	)

	IF @reportId IS NOT NULL
	BEGIN 
		UPDATE [Status]
		SET Label ='completed'
		WHERE Id = 
		(
			SELECT StatusId 
			FROM [Status] 
			JOIN Reports r ON r.StatusId = [Status].Id 
			WHERE r.Id = @reportId
		)
	END
END