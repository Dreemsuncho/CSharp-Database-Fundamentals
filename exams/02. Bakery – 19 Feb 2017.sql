-- Section 1. DDL (25 pts)

/* Problem 1 - Database Design.

-- You have been given the E/R Diagram of the bakery:
   Crate a database called Bakery. You need to create 7 tables:

    • Products – contains information about the products that are being sold in our bakery.
    • Ingredients – contains information about concrete fruits, vegetables, spices and so on. Has relation to both products and distributors.
    • ProductsIngredients – mapping table between products and ingredients.
    • Distributors – contains information about distributors – organizations that deliver ingredients.
    • Customers – contains information about the customers that use our products.
    • Countries – contains info for origin place (ingredients), general office(distributors) or homeland (customers).
    • Feedbacks – contains information about the feedback that the customers give while evaluating some of the products
*/
CREATE DATABASE Bakery
USE Bakery
GO

CREATE TABLE Countries 
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) UNIQUE
)

CREATE TABLE Customers 
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	Gender CHAR CHECK (Gender = 'M' OR Gender = 'F'),
	Age	INT,
	PhoneNumber CHAR(19),
	CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Products 
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) UNIQUE,
	[Description] NVARCHAR(250),
	Recipe NVARCHAR(MAX),
	Price MONEY CHECK (Price >= 0)
)

CREATE TABLE Feedbacks
(
	Id INT PRIMARY KEY IDENTITY,
	[Description] NVARCHAR(255),
	Rate DECIMAL(4, 2),
	ProductId INT FOREIGN KEY REFERENCES Products(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id)
)

CREATE TABLE Distributors
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(25) UNIQUE,
	AddressText NVARCHAR(30),
	Summary NVARCHAR(200),
	CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Ingredients
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(30),
	[Description] NVARCHAR(200),
	OriginCountryId INT FOREIGN KEY REFERENCES Countries(Id),
	DistributorId INT FOREIGN KEY REFERENCES Distributors(Id)
)

CREATE TABLE ProductsIngredients
(
	ProductId INT FOREIGN KEY REFERENCES Products(Id),
	IngredientId INT FOREIGN KEY REFERENCES Ingredients(Id),
	PRIMARY KEY (ProductId, IngredientId)
)



-- Section 2. DML (15 pts)
/*
   For this section put your queries in judge and use: “SQL Server run skeleton, run queries and check DB”.
   Before you start you have to import “Скелет”. If you have created the structure correctly the data should be successfully inserted.
   In this section, you have to do some data manipulations:
*/

/* Problem 2 - insert.

-- Let’s insert some sample data into the database. Write a query to add the following records into the corresponding tables. All Id’s should be auto-generated.
*/
INSERT INTO Distributors ([Name], CountryId, AddressText, Summary)
VALUES 
	('Deloitte & Touche', 2, '6 Arch St #9757', 'Customizable neutral traveling'),
	('Congress Title', 13, '58 Hancock St', 'Customer loyalty'),
	('Kitchen People', 1, '3 E 31st St #77', 'Triple-buffered stable delivery'),
	('General Color Co Inc', 21, '6185 Bohn St #72', 'Focus group'),
	('Beck Corporation', 23, '21 E 64th Ave', 'Quality-focused 4th generation hardware')

INSERT INTO Customers (FirstName, LastName, Age, Gender, PhoneNumber, CountryId)
VALUES 
	('Francoise', 'Rautenstrauch', 15, 'M', '0195698399', 5),
	('Kendra', 'Loud', 22, 'F', '0063631526', 1),
	('Lourdes', 'Bauswell', 50, 'M', '0139037043', 8),
	('Hannah', 'Edmison', 18, 'F', '0043343686', 1),
	('Tom', 'Loeza', 31, 'M', '0144876096', 23),
	('Queenie', 'Kramarczyk', 30, 'F', '0064215793', 29),
	('Hiu', 'Portaro', 25, 'M', '0068277755', 16),
	('Josefa', 'Opitz', 43, 'F', '0197887645', 17)



/* Problem 3 - Update.

-- We’ve decided to switch some of our ingredients to a local distributor. Update the table Ingredients and change the DistributorId of 
   "Bay Leaf", "Paprika" and "Poppy" to 35. Change the OriginCountryId to 14 of all ingredients with OriginCountryId equal to 8.
*/
UPDATE Ingredients
SET DistributorId = 35
WHERE [Name] IN ('Bay Leaf', 'Paprika', 'Poppy')

UPDATE Ingredients
SET OriginCountryId = 14
WHERE OriginCountryId = 8



/* Problem 4 - Delete.

-- Delete all Feedbacks which relate to Customer with Id 14 or to Product with Id 5.
*/
DELETE FROM Feedbacks
WHERE CustomerId = 14 OR ProductId = 5


-- Section 3. Querying (40 pts)
/*
   You need to start with a fresh dataset, so recreate your DB and import the sample data again.
   For this section put your queries in judge and use: “SQL Server prepare DB and run queries”.
*/

/* Problem 5 - Products by Price.

-- Select all products ordered by price (descending) then by name (ascending). 
   Required columns:
    • Name
    • Price
    • Description
    Example:
*/
SELECT 
	[Name], 
	Price, 
	[Description]
FROM 
	Products
ORDER BY
	Price DESC,
	[Name]



/* Problem 6 - Ingredients.

-- Find all ingredients coming from the countries with Id’s of 1, 10, 20. Order them by ingredient Id (ascending).
    Required columns:
    • Name
    • Description
    • OriginCountryId
*/
SELECT 
	[Name],
	[Description],
	OriginCountryId
FROM
	Ingredients
WHERE
	OriginCountryId IN (1, 10, 20)
ORDER BY
	Id



/* Problem 7 - Ingredients from Bulgaria and Greece.

-- Select top 15 ingredients coming from Bulgaria and Greece. Order them by ingredient name then by country name (both ascending).
   Required columns:
    • Name
    • Description
    • CountryName
*/
SELECT TOP 15
	i.[Name],
	i.[Description],
	c.[Name] AS CountryName
FROM
	Ingredients i
JOIN
	Countries c ON i.OriginCountryId = c.Id
WHERE
	c.[Name] IN ('Bulgaria', 'Greece')
ORDER BY
	i.[Name],
	c.[Name]



/* Problem 8 - Best Rated Products.

-- Select top 10 best rated products ordered by average rate (descending) then by amount of feedbacks (descending).
   Required columns:
    • Name
    • Description
    • AverageRate – average Rate for each product
    • FeedbacksAmount – number of feedbacks for each product
*/
SELECT TOP 10
	p.[Name],
	p.[Description],
	AVG(fb.Rate) AS AverageRate,
	COUNT(*) AS FeedbacksAmount
FROM
	Products p
JOIN
	Feedbacks fb ON fb.ProductId = p.Id
GROUP BY
	p.[Name],
	p.[Description]
ORDER BY
	AverageRate DESC,
	FeedbacksAmount DESC



/* Problem 9 - Negative Feedback

-- Select all feedbacks alongside with the customers which gave them. Filter only feedbacks which have rate below 5.0. Order results by 
   ProductId (descending) then by Rate (ascending).
   Required columns:
    • ProductId
    • Rate
    • Description
    • CustomerId
    • Age
    • Gender
*/
SELECT 
	fb.ProductId,
	fb.Rate,
	fb.[Description],
	c.Id AS CustomerId,
	c.Age,
	c.Gender
FROM
	Feedbacks fb
JOIN 
	Customers c ON fb.CustomerId = c.Id
WHERE
	fb.Rate < 5
ORDER BY
	ProductId DESC,
	fb.Rate



/* Problem 10 - Customers without Feedback.

-- Select all customers without feedbacks. Order them by customer id (ascending).
   Required columns:
    • CustomerName – customer’s first and last name, concatenated with space
    • PhoneNumber
    • Gender
*/
SELECT 
	CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
	c.PhoneNumber,
	c.Gender
FROM 
	Customers c
WHERE
	c.Id NOT IN (SELECT CustomerId FROM Feedbacks)
ORDER BY
	c.Id



/* Problem 11 - Honorable Mentions.

-- Select all feedbacks given by customers which have at least 3 feedbacks. Order them by product Id then by customer name and lastly by feedback id – all ascending.
   Required columns:
    • ProductId
    • CustomerName – customer’s first and last name, concatenated with space
    • FeedbackDescription
*/
SELECT 
	fb.ProductId,
	CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
	fb.[Description] AS FeedbackDescription
FROM 
	Customers c
JOIN 
	Feedbacks fb ON fb.CustomerId = c.Id
WHERE
	fb.CustomerId IN (SELECT CustomerId FROM Feedbacks GROUP BY CustomerId HAVING COUNT(CustomerId) >= 3)
ORDER BY
	fb.ProductId,
	CustomerName,
	fb.Id



/* Problem 12 - Customer by Criteria.

-- Select customers that are either at least 21 old and contain “an” in their first name or their phone number ends with “38” and are not from Greece. 
   Order by first name (ascending), then by age(descending).
   Required columns:
    • FirstName
    • Age
    • PhoneNumber
*/
SELECT 
	c.FirstName,
	c.Age,
	c.PhoneNumber
FROM
	Customers c
WHERE
	c.Age >= 21 AND
	(c.[FirstName] LIKE '%an%' OR c.PhoneNumber LIKE '%38') AND c.CountryId <> (SELECT Id FROM Countries WHERE [Name] = 'Greece')
ORDER BY
	c.FirstName,
	c.Age DESC



/* Problem 13 - Middle Range Distributors.

-- Select all distributors which distribute ingredients used in the making process of all products having average rate between 5 and 8 (inclusive). 
   Order by distributor name, ingredient name and product name all ascending.
   Required columns:
    • DistributorName
    • IngredientName
    • ProductName
    • AverageRate
*/
SELECT 
	d.[Name] AS DistributorName,
	i.[Name] AS IngredientName,
	p.[Name] AS ProductName,
	AVG(fb.Rate) AS AverageRate
FROM Distributors d
JOIN Ingredients i ON i.DistributorId = d.Id
JOIN ProductsIngredients [pi] ON [pi].IngredientId = i.Id
JOIN Products p ON p.Id = [pi].ProductId
JOIN Feedbacks fb ON fb.ProductId = p.Id
GROUP BY
	d.[Name],
	i.[Name], 
	p.[Name],
	i.Id
HAVING AVG(fb.Rate) BETWEEN 5 AND 8
ORDER BY
	d.[Name],
	i.[Name],
	p.[Name]



/* Problem 14 - The Most Positive Country.

-- Select the country which gave the most positive feedbacks. If there are several – print them all. Required columns:
    • CountryName
    • FeedbackRate – average feedback rate for each country
*/
SELECT TOP 1 WITH TIES
	co.[Name],
	AVG(Rate) AS FeedbackRate
FROM 
	Countries co
JOIN 
	Customers cu ON cu.CountryId = co.Id
JOIN 
	Feedbacks fb ON fb.CustomerId = cu.Id
GROUP BY
	co.[Name]
ORDER BY
	FeedbackRate DESC



/* Problem 15 - Country Representative.

-- Select all countries with their most active distributor (the one with the greatest number of ingredients). If there are several distributors with most 
   ingredients delivered, list them all. Order by country name then by distributor name.
   Required columns:
    • CountryName
    • DistributorName
*/	  
SELECT 
	CountryName, 
	DistributorName
FROM 
(
	SELECT 
		c.[Name] AS CountryName, 
		d.[Name] AS DistributorName,
	    DENSE_RANK() OVER (PARTITION BY c.[Name] ORDER BY COUNT(i.Id) DESC) AS [Rank]
	FROM 
		Countries c
	JOIN 
		Distributors d ON d.CountryId = c.Id
	JOIN 
		Ingredients i ON  i.DistributorId = d.Id
	GROUP BY
		c.[Name],
		d.[Name]
) AS crd
WHERE [Rank] = 1
ORDER BY 
	CountryName,
	DistributorName



-- Section 4. Programmability (20 pts)
/*
   For this section put your queries in judge and use: “SQL Server run skeleton, run queries and check DB”.
*/

/* Problem 16 - Customers with Countries.

-- Create a view named v_UserWithCountries which selects all customers with their countries.
   Required columns:
    • CustomerName – first name plus last name, with space between them
    • Age
    • Gender
    • CountryName
*/
GO
CREATE VIEW v_UserWithCountries
AS 
SELECT 
	CONCAT(cu.FirstName, ' ', cu.LastName) AS CustomerName,
	cu.Age,
	cu.Gender,
	co.[Name] AS CountryName
FROM 
	Customers cu
JOIN
	Countries co ON co.Id = cu.CountryId



/* Problem 17 - Feedback by Product Name.

-- Create a user defined function that receives a product’s name and returns its rating as a word, based on its average Rate. 
   For rates lower than 5, return "Bad", for rates between 5 and 8 return "Average" and for rates above 8, return "Good". If a product has no feedback, return "No rating".
   Parameters:
    • ProductName
*/
GO
CREATE FUNCTION udf_GetRating (@name VARCHAR(50))
RETURNS VARCHAR(9)
AS BEGIN
	DECLARE @result VARCHAR(9) = 
	(
		SELECT CASE 
			WHEN AVG(fb.Rate) < 5
				THEN 'Bad'
			WHEN AVG(fb.Rate) BETWEEN 5 AND 8
				THEN 'Average'
			WHEN AVG(fb.Rate) > 8
				THEN 'Good'
			ELSE 'No rating' END
		FROM Products p
		JOIN Feedbacks fb ON fb.ProductId = p.Id
		WHERE p.[Name] = @name
	)

	RETURN @result
END



/* Problem 18 - Send Feedback.

-- Each Customer should not have more than 3 feedbacks per product. Your task is to create a user defined procedure (usp_SendFeedback) which accepts customer’s 
   id, product’s id, rate and description.  You should insert the data but if the user already has 3 feedbacks – rollback any changes and throw an exception 
   with message “You are limited to only 3 feedbacks per product!” with Severity = 16 and State = 1.
*/
GO
CREATE PROC usp_SendFeedback 
	@customerId INT,
	@productId INT,
	@rate INT,
	@description VARCHAR(MAX)
AS BEGIN
	DECLARE @currentFeedbacksPerProduct INT = 
	(
		SELECT COUNT(*)
		FROM Feedbacks
		WHERE CustomerId = @customerId AND ProductId = @productId
	)

	IF @currentFeedbacksPerProduct >= 3
	BEGIN
		RAISERROR('You are limited to only 3 feedbacks per product!', 16, 1)
		RETURN
	END
	INSERT INTO Feedbacks 
		(CustomerId, ProductId, Rate, [Description])
	VALUES
		(@customerId, @productId, @rate, @description)
END



/* Problem 19 - Delete Products.

-- Create a trigger that deletes all of the relations of a product upon its deletion. 
*/
GO
CREATE TRIGGER tr_ReleaseReferences
ON Products
INSTEAD OF DELETE
AS BEGIN
	DECLARE @deletedId INT = (SELECT Id FROM DELETED)
	DELETE FROM Feedbacks WHERE ProductId = @deletedId
	DELETE FROM ProductsIngredients WHERE ProductId = @deletedId
	DELETE FROM Products WHERE Id = @deletedId
END



-- Section 5. Bonus (10 pts)

/* Problem 20 - Products by One Distributor.

-- Select all products which ingredients are delivered by only one distributor. Order them by product Id.
   Required columns:
    • ProductName
    • ProductAverageRate
    • DistributorName
    • DistributorCountry
*/
WITH cte
AS (
	SELECT
		p.[Name] AS ProductName,
		AVG(fb.Rate) AS ProductAverageRate,
		d.[Name] AS DistributorName,
		c.[Name] AS DistributorCountry,
		p.Id AS ProductId
	FROM 
		Products p
	JOIN 
		ProductsIngredients [pi] ON p.Id = [pi].ProductId
	JOIN
		Ingredients i ON i.Id = [pi].IngredientId
	JOIN
		Distributors d ON d.Id = i.DistributorId
	JOIN
		Feedbacks fb ON fb.ProductId = p.Id
	JOIN 
		Countries c ON c.Id = d.CountryId
	GROUP BY
		p.[Name],
		c.[Name],
		d.[Name],
		p.Id
)
SELECT
	cte.ProductName,
	cte.ProductAverageRate,
	cte.DistributorName,
	cte.DistributorCountry
FROM 
	cte
JOIN 
(
	SELECT 
		ProductName, 
		COUNT(DistributorName) DistCount
	FROM 
		cte
	GROUP BY
		ProductName
) AS DistCountTable ON DistCountTable.ProductName = cte.ProductName
WHERE 
	DistCountTable.DistCount = 1
ORDER BY 
	cte.ProductId