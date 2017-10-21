-- Section 1. DDL (30 pts)

/* Problem 1 - Database Design.

-- You have been given the E/R Diagram of the washing machine service:
   Crate a database called WMS. You need to create 9 tables:

    • Clients – contains information about the customers that use the service
    • Mechanics – contains information about employees
    • Jobs – contains information about all machines that clients submitted for repairs
    • Models – list of all washing machine models that the servie operates with
    • Orders – contains information about orders for parts
    • Parts – list of all parts the service operates with
    • OrderParts – mapping table between Orders and Parts with additional Quantity field
    • PartsNeeded – mapping table between Jobs and Parts with additional Quantity field
    • Vendors – list of vendors that supply parts to the service

   Include the following fields in each table. Unless otherwise specified, all fields are required.
*/

CREATE DATABASE WMS
GO
USE WMS
GO

CREATE TABLE Clients 
(
	ClientId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	Phone NVARCHAR(12) CHECK (LEN(Phone) = 12) NOT NULL
)

CREATE TABLE Mechanics
(
	MechanicId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	[Address] VARCHAR(255) NOT NULL
)

CREATE TABLE Models
(
	ModelId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) UNIQUE NOT NULL,
)

CREATE TABLE Jobs 
(
	JobId INT PRIMARY KEY IDENTITY,
	ModelId INT FOREIGN KEY REFERENCES Models(ModelId) NOT NULL,
	[Status] VARCHAR(11) CHECK ([Status] IN ('Pending', 'In Progress', 'Finished')) DEFAULT 'Pending' NOT NULL,
	[ClientId] INT FOREIGN KEY REFERENCES Clients(ClientId) NOT NULL,
	MechanicId INT FOREIGN KEY REFERENCES Mechanics(MechanicId),
	IssueDate DATE NOT NULL,
	FinishDate DATE,
)

CREATE TABLE Vendors 
(
	VendorId INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) UNIQUE NOT NULL
)

CREATE TABLE Parts
(
	PartId INT PRIMARY KEY IDENTITY,
	SerialNumber VARCHAR(50) UNIQUE NOT NULL,
	[Description] VARCHAR(255),
	Price MONEY CHECK (Price <= 9999.99 AND Price > 0) NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors(VendorId) NOT NULL,
	StockQty INT CHECK (StockQty >= 0) NOT NULL DEFAULT 0
)

CREATE TABLE Orders
(
	OrderId INT PRIMARY KEY IDENTITY,
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	IssueDate DATE,
	Delivered BIT NOT NULL DEFAULT 0
)

CREATE TABLE OrderParts
(
	OrderId INT FOREIGN KEY REFERENCES Orders(OrderId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	Quantity INT CHECK(Quantity > 0) NOT NULL DEFAULT 1,
	PRIMARY KEY (OrderId, PartId)
)

CREATE TABLE PartsNeeded
(
	JobId INT FOREIGN KEY REFERENCES Jobs(JobId) NOT NULL,
	PartId INT FOREIGN KEY REFERENCES Parts(PartId) NOT NULL,
	Quantity INT CHECK(Quantity > 0) NOT NULL DEFAULT 1,
	PRIMARY KEY (JobId, PartId)
)



-- Section 2. DML (10 pts)
/*
   Before you start you have to import Data.sql. If you have created the structure 
   correctly the data should be successfully inserted.
   In this section, you have to do some data manipulations:
*/

/* Problem 2 - Insert.

-- Let’s insert some sample data into the database. Write a query to add the following records into the corresponding tables. 
   All Id’s should be auto-generated. Replace names that relate to other tables with the appropriate ID (look them up manually, 
   there is no need to perform table joins).
*/
INSERT INTO Clients (FirstName, LastName, Phone)
VALUES
	('Teri', 'Ennaco', '570-889-5187'),
	('Merlyn', 'Lawler', '201-588-7810'),
	('Georgene', 'Montezuma', '925-615-5185'),
	('Jettie', 'Mconnell', '908-802-3564'),
	('Lemuel', 'Latzke', '631-748-6479'),
	('Melodie', 'Knipp', '805-690-1682'),
	('Candida',	'Corbley', '908-275-8357')

INSERT INTO Parts (SerialNumber, [Description], Price, VendorId)
VALUES
	('WP8182119', 'Door Boot Seal', 117.86, (SELECT VendorId FROM Vendors WHERE [Name] = 'Suzhou Precision Products')),
	('W10780048', 'Suspension Rod', 42.81, (SELECT VendorId FROM Vendors WHERE [Name] = 'Shenzhen Ltd.')),
	('W10841140', 'Silicone Adhesive ', 6.77, (SELECT VendorId FROM Vendors WHERE [Name] = 'Fenghua Import Export')),
	('WPY055980', 'High Temperature Adhesive', 13.94, (SELECT VendorId FROM Vendors WHERE [Name] = 'Qingdao Technology'))



/* Problem 3 - Update.

-- Assign all Pending jobs to the mechanic Ryan Harnos (look up his ID manually, there is no need to use table joins) 
   and change their status to 'In Progress'.
*/
UPDATE 
	Jobs
SET 
	MechanicId = (SELECT MechanicId FROM Mechanics WHERE (FirstName + ' ' + LastName = 'Ryan Harnos')),
	[Status] = 'In Progress'
WHERE 
	[Status] = 'Pending'



/* Problem 4 - Delete.

-- Cancel Order with ID 19 – delete the order from the database and all associated entries from the mapping table.
*/
DELETE FROM OrderParts
WHERE OrderId = 19

DELETE FROM Orders
WHERE  OrderId = 19



-- Section 3. Querying (45 pts)
/*
   You need to start with a fresh dataset, so run the Data.sql script again. It includes a section that 
   will delete all records and replace them with the starting set, so you don’t need to drop your database.
*/

/* Problem 5 - Clients By Name.

-- Select all clients ordered by last name (ascending) then by client ID (ascending). 
   Required columns:
    • First Name
    • Last Name
    • Phone
*/
SELECT 
	FirstName, 
	LastName, 
	Phone
FROM
	Clients
ORDER BY 
	LastName, 
	Phone



/* Problem 6 - Job Status.

-- Find all active jobs (that aren’t Finished) and display their status and issue date. 
   Order by issue date and by job ID (both ascending).
*/
SELECT [Status], IssueDate
FROM Jobs
WHERE [Status] <> 'Finished'
ORDER BY IssueDate, JobId



/* Problem 7 - Mechanic Assignments.

-- Select all mechanics with their jobs. Include job status and issue date. Order by mechanic Id, issue date, job Id (all ascending).
   Required columns:
    • Mechanic Full Name
    • Job Status
    • Job Issue Date
*/
SELECT 
	(m.FirstName + ' ' + m.LastName) AS [Mechanic],
	j.[Status],
	j.IssueDate
FROM 
	Mechanics m
JOIN 
	Jobs j ON j.MechanicId = m.MechanicId
ORDER BY 
	m.MechanicId,
	j.IssueDate,
	j.JobId



/* Problem 8 - Current Clients.

-- Select the names of all clients with active jobs (not Finished). Include the status of the job and how many days it’s 
   been since it was submitted. Assume the current date is 24 April 2017. Order results by time length (descending) 
   and by client ID (ascending).
   Required columns:
    • Client Full Name
    • Days going – how many days have passed since the issuing
    • Status
*/
SELECT
	(c.FirstName + ' ' + c.LastName) AS Client,
	DATEDIFF(DAY, j.IssueDate, CAST('04/24/2017' AS DATE)) AS [Days going],
	j.[Status]
FROM
	Clients c
JOIN
	Jobs j ON c.ClientId = j.ClientId
WHERE
	j.[Status] <> 'Finished'
ORDER BY
	[Days going] DESC,
	c.ClientId



/* Problem 9 - Mechanic Performance.

-- Select all mechanics and the average time they take to finish their assigned jobs. Calculate the average as an integer. 
   Order results by mechanic ID (ascending).
   Required columns:
    • Mechanic Full Name
    • Average Days – average number of days the machanic took to finish the job
*/
SELECT 
	(m.FirstName + ' ' + m.LastName) AS [Mechanic],
	AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)) AS [Average Days]
FROM
	Mechanics m
JOIN
	Jobs j ON m.MechanicId = j.MechanicId
GROUP BY
	m.FirstName,
	m.LastName,
	m.MechanicId
ORDER BY
	m.MechanicId



/* Problem 10 - Hard Earners.

-- Select the first 3 mechanics who have more than 1 active job (not Finished). Order them by number of jobs 
   (descending) and by mechanic ID (ascending).
   Required columns:
    • Mechanic Full Name
    • Number of Jobs
*/
SELECT 
	(m.FirstName + ' ' + m.LastName) AS Mechanic,
	COUNT(j.JobId) AS Jobs
FROM
	Mechanics m
JOIN 
	Jobs j ON m.MechanicId = j.MechanicId
GROUP BY
	m.FirstName,
	m.LastName,
	j.[Status],
	m.MechanicId
HAVING
	j.[Status] <> 'Finished' AND
	COUNT(j.JobId) > 1
ORDER BY
	Jobs DESC,
	m.MechanicId



/* Problem 11 - Available Mechanics.

-- Select all mechanics without active jobs (include mechanics which don’t have any 
   job assigned or all of their jobs are finished). Order by ID (ascending).
*/
GO
SELECT
	(FirstName + ' ' + LastName) AS Available
FROM
	Mechanics
WHERE
	MechanicId NOT IN
	(
		SELECT DISTINCT MechanicId 
		FROM Jobs 
		WHERE MechanicId IS NOT NULL AND [Status] <> 'Finished'
	)
ORDER BY
	MechanicId



/* Problem 12 - Parts Cost.

-- Display the total cost of all parts ordered during the last three weeks. Assume the current date is 24 April 2017.
   Required columns:
   • Parts Total Cost
*/
SELECT 
	ISNULL(SUM(p.Price * op.Quantity), 0) AS [Parts Total]
FROM 
	Parts p
JOIN
	OrderParts op ON p.PartId = op.PartId
JOIN
	Orders o ON op.OrderId = o.OrderId
WHERE
	DATEDIFF(WEEK, o.IssueDate, CAST('04/24/2017' AS DATE)) <= 3



/* Problem 13 - Past Exspenses.

-- Select all finished jobs and the total cost of all parts that were ordered for them. 
   Sort by total cost of parts ordered (descending) and by job ID (ascending).
   Required columns:
   • Job ID
   • Total Parts Cost
*/
SELECT 
	j.JobId, 
	ISNULL(SUM(p.Price * op.Quantity), 00.00) AS Total
FROM
	Jobs j
LEFT JOIN
	Orders o ON j.JobId = o.JobId
LEFT JOIN
	OrderParts op ON o.OrderId = op.OrderId
LEFT JOIN
	Parts p ON op.PartId = p.PartId
WHERE
	j.FinishDate IS NOT NULL AND j.[Status] = 'Finished'
GROUP BY
	j.JobId
ORDER BY
	Total DESC,
	j.JobId



/* Problem 14 - Model Repair Time.

-- Select all models with the average time it took to service, out of all the times it was repaired. Calculate the average as an integer value. Order the results by average service time ascending.
    Required columns:
    • Model ID
    • Name
    • Average Service Time – average number of days it took to finish the job; note the word 'days' attached at the end!
*/
SELECT 
	m.ModelId,
	m.[Name],
	CONCAT(AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate)), ' days' ) AS [Average Service Time]
FROM
	Models m
JOIN
	Jobs j ON m.ModelId = j.ModelId
GROUP BY
	m.ModelId,
	m.[Name]
ORDER BY
	AVG(DATEDIFF(DAY, j.IssueDate, j.FinishDate))



/* Problem 15 - Faultiest Model.

-- Find the model that breaks the most (has the highest number of jobs associated with it). Include the cost of parts ordered for it. If there are more than one models that were serviced the same number of times, list them all.
   Required columns:
    • Name
    • Times Serviced – number of assiciated jobs
    • Parts Total – cost of all parts ordered for the jobs
*/
SELECT TOP 1 WITH TIES
	m.[Name] AS Model,
	COUNT(*) AS [Times Servied],
	ISNULL((
		SELECT SUM(p.Price * Quantity)
		FROM Jobs j
		JOIN Orders o ON j.JobId = o.JobId
		JOIN OrderParts op ON op.OrderId = o.OrderId
		JOIN Parts p ON p.PartId = op.PartId
		WHERE j.ModelId = m.ModelId
	), 0) AS [Parts Total]
FROM
	Models m
JOIN
	Jobs j ON m.ModelId = j.ModelId
GROUP BY
	m.[Name],
	m.ModelId
ORDER BY
	[Times Servied] DESC



/* Problem 16 - Missing Parts.

-- List all parts that are needed for active jobs (not Finished) without sufficient quantity in stock and in pending 
   orders (the sum of parts in stock and parts ordered is less than the required quantity). Order them by part ID (ascending).
   Required columns:
    • Part ID
    • Description
    • Required – number of parts required for active jobs
    • In Stock – how many of the part are currently in stock
    • Ordered – how many of the parts are expected to be delivered (associated with order that is not Delivered)
*/
SELECT
	p.PartId,
	p.[Description],
	pn.Quantity AS [Required],
	p.StockQty AS [In Stock],
	ISNULL(op.Quantity, 0) AS Ordered
FROM
	Parts p
JOIN
	PartsNeeded pn ON pn.PartId = p.PartId
JOIN
	Jobs j ON j.JobId = pn.JobId
LEFT JOIN
	Orders o ON o.JobId = j.JobId
LEFT JOIN 
	OrderParts op ON op.OrderId = o.OrderId
WHERE
	j.[Status] <> 'Finished' AND 
	(p.StockQty + ISNULL(op.Quantity, 0)) < pn.Quantity
ORDER BY
	p.PartId



-- Section 4. Programmability (15 pts)

/* Problem 17 - Cost of Order.

-- Create a user defined function (udf_GetCost) that receives a job’s ID and returns the total cost of all parts 
   that were ordered for it. Return 0 if there are no orders.
   Parameters:
   • JobId
*/
GO
ALTER FUNCTION udf_GetCost(@jobID INT)
RETURNS DECIMAL(10,2)
AS BEGIN
	DECLARE @Total DECIMAL(10,2) = 
	(
		SELECT ISNULL(SUM(p.Price), 0)
		FROM Parts p
		JOIN OrderParts op ON op.PartId = p.PartId
		JOIN Orders o ON o.OrderId = op.OrderId
		JOIN Jobs j ON j.JobId = o.JobId AND j.JobId = @jobID
	)
	RETURN @Total
END



/* Problem 18 - Place Order.

-- Your task is to create a user defined procedure (usp_PlaceOrder) which accepts job ID, part serial number and 
   quantity and creates an order with the specified parameters. If an order already exists for the given job that 
   and the order is not issued (order’s issue date is NULL), add the new product to it. If the part is already listed in the order, 
   add the quantity to the existing one.
   When a new order is created, set it’s IssueDate to NULL.
   Limitations:
    • An order cannot be placed for a job that is Finished; error message ID 50011 "This job is not active!"
    • The quantity cannot be zero or negative; error message ID 50012 "Part quantity must be more than zero!"
    • The job with given ID must exist in the database; error message ID 50013 "Job not found!"
    • The part with given serial number must exist in the database ID 50014 "Part not found!"
   If any of the requirements aren’t met, rollback any changes to the database you’ve made and throw an exception with the 
   appropriate message and state 1. 
   Parameters:
    • JobId
    • Part Serial Number
    • Quantity
*/
GO
CREATE PROC usp_PlaceOrder
	@jobId INT,
	@partSerialNumber VARCHAR(50),
	@quantity INT
AS BEGIN
	IF @quantity < 1
	BEGIN
		RAISERROR('Part quantity must be more than zero!', 16, 1)
		RETURN
	END

	DECLARE @jobIdExist INT = (SELECT JobId FROM Jobs WHERE JobId = @jobId)
	IF @jobIdExist IS NULL
	BEGIN 
		RAISERROR('Job not found!', 16, 1)
		RETURN
	END
	
	DECLARE @jobStatus VARCHAR(11) = (SELECT [Status] FROM Jobs WHERE JobId = @jobId)
	IF @jobStatus = 'Finished'
	BEGIN
		RAISERROR('This job is not active!', 16, 1)
		RETURN
	END

	DECLARE @partId INT = (SELECT PartId FROM Parts WHERE SerialNumber = @partSerialNumber)
	IF @partId IS NULL
	BEGIN
		RAISERROR('Part not found!', 16, 1)
		RETURN
	END


	DECLARE @orderId INT = 
	(
		SELECT o.OrderId 
		FROM Orders o
		JOIN OrderParts op ON o.OrderId = op.OrderId
		JOIN Parts p ON op.PartId = p.PartId
		WHERE o.JobId = @jobId AND o.IssueDate IS NULL AND p.PartId = @partId
	)

	if @orderId IS NULL
	BEGIN 
		INSERT INTO Orders (JobId, IssueDate)
		VALUES (@jobId, NULL)

		INSERT INTO OrderParts (OrderId, PartId, Quantity)
		VALUES (IDENT_CURRENT('Orders'), @partId, @quantity)
	END
	ELSE
	BEGIN
		DECLARE @orderPartsExist INT = (SELECT @@ROWCOUNT FROM OrderParts WHERE OrderId = @orderId AND PartId = @partId)
		IF @orderPartsExist IS NULL
		BEGIN
			INSERT INTO OrderParts (OrderId, PartId, Quantity)
			VALUES (@orderId, @partId, @quantity)
		END
		ELSE
		BEGIN
			UPDATE OrderParts
			SET Quantity += @quantity
			WHERE OrderId = @orderId AND PartId = @partId
		END
	END
END



/* Problem 19 - Detect Delivery.

-- Create a trigger that detects when an order’s delivery status is changed from False to True which adds 
   the quantities of all ordered parts to their stock quantity value (Qty).
*/
GO
CREATE OR ALTER TRIGGER tr_DetectOrderStatus
ON Orders
AFTER UPDATE
AS BEGIN
	DECLARE @beforeStatus BIT = (SELECT Delivered FROM DELETED)
	DECLARE @afterStatus BIT = (SELECT Delivered FROM INSERTED)

	if @beforeStatus = 0 AND @afterStatus = 1 
	BEGIN
		UPDATE Parts
		SET StockQty += op.Quantity
		FROM Parts p
		JOIN OrderParts op ON p.PartId = op.PartId
		JOIN Orders o ON op.OrderId = o.OrderId
		JOIN INSERTED i ON i.OrderId = o.OrderId
		JOIN DELETED d ON i.OrderId = d.OrderId
	END
END



-- Section 5. Bonus (10 pts)

/* Problem 20 - Vendor Preference.

-- List all mechanics and their preference for each vendor as a percentage of parts’ quantities they ordered for their jobs. 
   Express the percentage as an integer value. Order them by mechanic’s full name (ascending), number of parts from each 
   vendor (descending) and by vendor name (ascending).
   Required columns:
    • Mechanic Full Name
    • Vendor Name
    • Parts ordered from vendor
    • Preference for Vendor (percantage of parts out of all parts count ordered by the mechanic)
*/
GO
WITH cte_cte
AS 
(
	SELECT 
		m.MechanicId,
		v.VendorId,
		SUM(op.Quantity) AS VParts
	FROM Mechanics m
	JOIN Jobs j ON j.MechanicId = m.MechanicId
	JOIN Orders o ON o.JobId = j.JobId
	JOIN OrderParts op ON op.OrderId = o.OrderId
	JOIN Parts p ON p.PartId = op.PartId
	JOIN Vendors v ON v.VendorId = p.VendorId
	GROUP BY
		m.MechanicId,
		v.VendorId
)
SELECT 
	(m.FirstName + ' ' + m.LastName) AS Mechanic,
	v.[Name] AS Vendor,
	cte.VParts AS Parts,
	CAST(CAST((CAST(VParts AS DECIMAL(6,2)) / (SELECT SUM(VParts) FROM cte_cte WHERE MechanicId = m.MechanicId) * 100) AS INT) AS VARCHAR(50)) + '%' AS Preference
FROM
	Mechanics m
JOIN 
	cte_cte cte ON cte.MechanicId = m.MechanicId
JOIN	
	Vendors v ON v.VendorId = cte.VendorId
GROUP BY
	(m.FirstName + ' ' + m.LastName),
	v.[Name],
	cte.VParts,m.MechanicId
ORDER BY 
	(m.FirstName + ' ' + m.LastName),
	Parts DESC,
	Vendor