-- PART I – Queries for Diablo Database
USE Diablo
GO
/* Problem 1 - Number of users for email provider.

-- Find number of users for email provider from the largest to smallest, then by Email Provider in ascending order. 
*/
CREATE FUNCTION ufn_GetEmailProvider(@email NVARCHAR(40))
RETURNS NVARCHAR(40)
AS
BEGIN
	RETURN SUBSTRING(@email, CHARINDEX('@', @email) + 1, LEN(@email))
END
GO

SELECT 
	[Email Provider], 
	COUNT(*) AS [Number Of Users]
FROM
(
	SELECT 
		dbo.ufn_GetEmailProvider(Email) AS [Email Provider]
	FROM 
		Users
) AS Users
GROUP BY 
	[Email Provider]
ORDER BY
	[Number Of Users] DESC, 
	[Email Provider]



/* Problem 2 - All User in Games.

-- Find all user in games with information about them. Display the game name, game type, username, level, cash and character name. 
   Sort the result by level in descending order, then by username and game in alphabetical order.
*/
SELECT 
	Game, 
	[Game Type], 
	Username, 
	[Level], 
	Cash, 
	[Character]
FROM
(
	SELECT 
		g.[Name] AS Game, 
		gt.[Name] AS [Game Type], 
		u.Username, 
		ug.[Level] AS [Level], 
		ug.Cash, 
		c.[Name] AS [Character]
	FROM
		Games g
	JOIN 
		UsersGames ug ON g.Id = ug.GameId
	JOIN 
		Characters c ON ug.CharacterId = c.Id
	JOIN
		GameTypes gt ON g.GameTypeId = gt.Id
	JOIN 
		Users u ON ug.UserId = u.Id
) AS UsersStatistics
ORDER BY 
	[Level] DESC, 
	Username, 
	Game



/* Problem 3 - Users in Games with Their Items.

-- Find all users in games with their items count and items price. Display the username, game name, 
   items count and items price. Display only user in games with items count more or equal to 10. 
   Sort the results by items count in descending order then by price in descending order and by username in ascending order.
*/
SELECT 
	u.Username, 
	g.[Name] AS Game,
	COUNT(i.Id) AS [Items Count],
	SUM(i.Price) AS [Items Price]
FROM
	Games g
JOIN
	UsersGames ug ON g.Id = ug.GameId
JOIN 
	UserGameItems ugi ON ug.Id = ugi.UserGameId
JOIN
	Users u	ON u.Id = ug.UserId
JOIN 
	Items i ON ugi.ItemId = i.Id
GROUP BY 
	u.Username, 
	g.[Name]
HAVING 
	COUNT(i.Id) >= 10
ORDER BY 
	[Items Count] DESC, 
	[Items Price] DESC, 
	u.Username



/* Problem 4 - * User in Games with Their Statistics.

-- Find information about every game a user has played with their statistics. Each user may have participated in several games. 
   Display the username, game name, character name, strength, defence, speed, mind and luck.
   Every statistic (strength, defence, speed, mind and luck) should be a sum of the character statistic, 
   game type statistic and items for user in game statistic. One user may have multiple characters in a single game. 
   What you should do in order to calculate the statistic properly is to sum the following: 
    1. Get the sum of all items - of all characters that the user may have(SUM).
    2. For all of his characters get the character stats which are the biggest (MAX).
    3. For all of his game types stats select only these which are again the biggest (MAX).
   Order the results by Strength, Defence, Speed, Mind, Luck – all in descending order. 
*/ 
WITH CTE_stats
AS
(
	SELECT
		ug.UserId,
		ug.GameId,
		SUM(s.Strength) AS Strength,
		SUM(s.Defence) AS Defence,
		SUM(s.Speed) AS Speed,
		SUM(s.Mind) AS Mind,
		SUM(s.Luck) AS Luck
	FROM
		UsersGames ug
	JOIN
		UserGameItems ugi ON ugi.UserGameId = ug.Id
	JOIN
		Items i ON i.Id = ugi.ItemId
	JOIN
		[Statistics] s ON s.Id = i.StatisticId
	GROUP BY
		ug.UserId,
		ug.GameId
)
SELECT
	u.UserName,
	g.[Name] AS Game,
	MAX(c.[Name]) AS [Character],
	MAX(CTE_stats.Strength) + MAX(gts.Strength) + MAX(cts.Strength) AS Strength,
	MAX(CTE_stats.Defence) + MAX(gts.Defence) + MAX(cts.Defence) AS Defence,
	MAX(CTE_stats.Speed) + MAX(gts.Speed) + MAX(cts.Speed) AS Speed,
	MAX(CTE_stats.Mind) + MAX(gts.Mind) + MAX(cts.Mind) AS Mind,
	MAX(CTE_stats.Luck) + MAX(gts.Luck) + MAX(cts.Luck) AS Luck
FROM
	Users u
JOIN
	UsersGames ug ON ug.UserId = u.Id
JOIN
	Characters c ON c.Id = ug.CharacterId
JOIN
	Games g ON g.Id = ug.GameId
JOIN
	UserGameItems ugi ON ugi.UserGameId = ug.Id
JOIN
	Items i ON i.Id = ugi.ItemId
JOIN
	GameTypes gt ON gt.Id = g.GameTypeId
JOIN
	CTE_stats ON CTE_stats.UserId = u.Id AND
				 CTE_stats.GameId = g.Id
JOIN
	[Statistics] gts ON gts.Id = gt.BonusStatsId
JOIN
	[Statistics] cts ON cts.Id = c.StatisticId
GROUP BY
	u.Username,
	g.[Name]
ORDER BY
	Strength DESC, 
	Defence DESC,
	Speed DESC,
	Mind DESC,
	Luck DESC



/* Problem 5 - All Items with Greater than Average Statistics.

-- Find all items with statistics larger than average. Display only items that have Mind, Luck and Speed 
   greater than average Items mind,  luck and speed. Sort the results by item names in alphabetical order.
*/
SELECT 
	i.[Name], 
	i.Price, 
	i.MinLevel, 
	s.Strength, 
	s.Defence, 
	s.Speed,
	s.Luck,
	s.Mind
FROM
	Items i
JOIN
	[Statistics] s ON i.StatisticId = s.Id	
WHERE 
	s.Mind > (SELECT AVG(Mind) FROM [Statistics]) AND
	s.Luck > (SELECT AVG(Luck) FROM [Statistics]) AND
	s.Speed > (SELECT AVG(Speed) FROM [Statistics])
ORDER BY
	i.[Name]



/* Problem 6 - Display All Items with Information about Forbidden Game Type.

-- Find all items and information whether and what forbidden game types they have. Display item name, 
   price, min level and forbidden game type. Display all items. Sort the results by game type in descending order, 
   then by item name in ascending order. 
*/
SELECT 
	i.[Name] AS Item,
	i.Price,
	i.MinLevel,
	gt.[Name] AS [Forbidden Game Type]
FROM
	Items i
LEFT JOIN
	GameTypeForbiddenItems gtfb	ON i.Id = gtfb.ItemId
LEFT JOIN
	GameTypes gt ON gtfb.GameTypeId = gt.Id
ORDER BY
	gt.[Name] DESC,
	i.[Name]



/* Problem 7 - Buy Items for User in Game.

-- 1. User Alex is in the shop in the game “Edinburgh” and she wants to buy some items. 
   She likes Blackguard, Bottomless Potion of Amplification, Eye of Etlich (Diablo III), 
   Gem of Efficacious Toxin, Golden Gorget of Leoric and Hellfire Amulet. Buy the items. 
   You should add the data in the right tables. Get the money for the items from user in game Cash.

-- 2. Select all users in the current game with their items. 
   Display username, game name, cash and item name. Sort the result by item name.
*/
-- 7.1
DECLARE @userGameId INT = 
	(
		SELECT 
			Id
		FROM 
			UsersGames
		WHERE
			GameId = (SELECT Id FROM Games WHERE [Name] = 'Edinburgh') AND
			UserId = (SELECT Id FROM Users Where Username = 'Alex')	
	)

DECLARE @userItemsPrice MONEY =
	(
		SELECT 
			SUM(i.Price)
		FROM 
			Items i
		WHERE
			i.[Name] IN 
			(
				'Blackguard', 
				'Bottomless Potion of Amplification', 
				'Eye of Etlich (Diablo III)', 
				'Gem of Efficacious Toxin', 
				'Golden Gorget of Leoric', 
				'Hellfire Amulet'
			)
	)

INSERT INTO
	UserGameItems(ItemId, UserGameId)
SELECT 
	i.Id, @userGameId	
FROM
	Items i
WHERE 
	i.[Name] IN 
	(
		'Blackguard', 
		'Bottomless Potion of Amplification', 
		'Eye of Etlich (Diablo III)', 
		'Gem of Efficacious Toxin', 
		'Golden Gorget of Leoric', 
		'Hellfire Amulet'
	)
	
UPDATE 
	UsersGames
SET
	Cash -= @userItemsPrice
WHERE 
	Id = @userGameId

-- 7.2
SELECT
	u.Username,
	g.[Name],
	ug.Cash,
	i.[Name] AS [Item Name]
FROM
	UsersGames ug
JOIN 
	Games g	ON ug.GameId = g.Id
JOIN 
	Users u	ON u.Id = ug.UserId
JOIN
	UserGameItems ugi ON ugi.UserGameId = ug.Id
JOIN
	Items i ON i.Id = ugi.ItemId
WHERE 
	g.[Name] = 'Edinburgh'
ORDER BY
	[Item Name]



-- PART II – Queries for Geography Database
USE [Geography]
GO
/* Problem 8 - Peaks and Mountains.

-- Find all peaks along with their mountain sorted by elevation (from the highest to the lowest), 
   then by peak name alphabetically. Display the peak name, mountain range name and elevation.
*/
SELECT
	p.PeakName,
	m.MountainRange AS Mountain,
	p.Elevation
FROM
	Peaks p
JOIN 
	Mountains m ON p.MountainId = m.Id
ORDER BY
	p.Elevation DESC,
	p.PeakName



/* Problem 9 - Peaks with Their Mountains, Country and Continent.

-- Find all peaks along with their mountain, country and continent. When a mountain belongs to multiple countries, 
   display them all. Sort the results by peak name alphabetically, then by country name alphabetically.
*/
SELECT
	p.PeakName, 
	m.MountainRange, 
	c.CountryName, 
	ct.ContinentName
FROM
	Peaks p
JOIN 
	Mountains m ON p.MountainId = m.Id
JOIN
	MountainsCountries mc ON m.Id = mc.MountainId
JOIN
	Countries c ON mc.CountryCode = c.CountryCode
JOIN
	Continents ct ON ct.ContinentCode = c.ContinentCode
ORDER BY 
	p.PeakName,
	c.CountryName



/* Problem 10 - Rivers by Country.

-- For each country in the database, display the number of rivers passing through that country and 
   the total length of these rivers. When a country does not have any river, display 0 as rivers count and as total length. 
   Sort the results by rivers count (from largest to smallest), then by total length (from largest to smallest), 
   then by country alphabetically.
*/
SELECT
	 c.CountryName,
	 ctn.ContinentName,
	 COUNT(r.Id) AS [Rivers Count],
	 ISNULL(SUM(r.[Length]), 0) AS [Total Length]
FROM 
	Countries c 
LEFT JOIN
	Continents ctn ON ctn.ContinentCode = c.ContinentCode
LEFT JOIN
	CountriesRivers cr ON cr.CountryCode = c.CountryCode
LEFT JOIN
	Rivers r ON r.Id = cr.RiverId
GROUP BY
	c.CountryName,
	ctn.ContinentName
ORDER BY
	[Rivers Count] DESC,
	[Total Length] DESC,
	c.CountryName



/* Problem 11 - Count of Countries by Currency

-- Find the number of countries for each currency. Display three columns: currency code, 
   currency description and number of countries. Sort the results by number of countries (from highest to lowest), 
   then by currency description alphabetically. Name the columns exactly like in the table below.
*/
SELECT 
	cu.CurrencyCode, 
	cu.[Description] AS Currency, 
	COUNT(c.CountryCode) AS NumberOfCountries
FROM
	Currencies cu
LEFT JOIN
	Countries c ON cu.CurrencyCode = c.CurrencyCode
GROUP BY 
	cu.CurrencyCode,
	cu.[Description]
ORDER BY
	NumberOfCountries DESC,
	Currency



/* Problem 12 - Population and Area by Continent

-- For each continent, display the total area and total population of all its countries. 
   Sort the results by population from highest to lowest.
*/
SELECT 
	co.ContinentName,
	SUM(CONVERT(BIGINT, c.AreaInSqKm)) AS CountriesArea,
	SUM(CONVERT(BIGINT, c.[Population])) AS CountriesPopulation
FROM
	Continents co
JOIN
	Countries c ON co.ContinentCode = c.ContinentCode
GROUP BY 
	co.ContinentName
ORDER BY
	CountriesPopulation DESC



/* Problem 13 - Monasteries by Country.

-- 1. Create a table Monasteries(Id, Name, CountryCode). Use auto-increment for the primary key. 
   Create a foreign key between the tables Monasteries and Countries.

-- 2. Execute the following SQL script (it should pass without any errors):

-- 3. Write a SQL command to add a new Boolean column IsDeleted in the Countries table (defaults to false). 
   Note that there is no "Boolean" type in SQL server, so you should use an alternative and make sure you 
   set the default value properly.

-- 4. Write and execute a SQL command to mark as deleted all countries that have more than 3 rivers.

-- 5. Write a query to display all monasteries along with their countries sorted by monastery name. 
   Skip all deleted countries and their monasteries.
*/
-- 13.1
CREATE TABLE Monasteries
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(100),
	CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode),
)

-- 13.2
INSERT INTO Monasteries(Name, CountryCode) VALUES
('Rila Monastery “St. Ivan of Rila”', 'BG'), 
('Bachkovo Monastery “Virgin Mary”', 'BG'),
('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
('Kopan Monastery', 'NP'),
('Thrangu Tashi Yangtse Monastery', 'NP'),
('Shechen Tennyi Dargyeling Monastery', 'NP'),
('Benchen Monastery', 'NP'),
('Southern Shaolin Monastery', 'CN'),
('Dabei Monastery', 'CN'),
('Wa Sau Toi', 'CN'),
('Lhunshigyia Monastery', 'CN'),
('Rakya Monastery', 'CN'),
('Monasteries of Meteora', 'GR'),
('The Holy Monastery of Stavronikita', 'GR'),
('Taung Kalat Monastery', 'MM'),
('Pa-Auk Forest Monastery', 'MM'),
('Taktsang Palphug Monastery', 'BT'),
('Sümela Monastery', 'TR')

-- 13.3
ALTER TABLE 
	Countries
ADD
	IsDeleted BIT DEFAULT 0 NOT NULL

-- 13.4
UPDATE 
	Countries
SET
	IsDeleted = 1
WHERE
	CountryCode IN 
	(
		SELECT 
			c.CountryCode
		FROM 
			Countries c
		JOIN 
			CountriesRivers cr ON  cr.CountryCode = c.CountryCode
		JOIN
			Rivers r ON r.Id = cr.RiverId
		GROUP BY
			c.CountryCode
		HAVING
			COUNT(r.Id) > 3
	)
	
-- 13.5
SELECT 
	m.[Name] AS Monastery,
	c.CountryName AS Country
FROM
	Monasteries m
JOIN
	Countries c ON m.CountryCode = c.CountryCode
WHERE
	c.IsDeleted = 0
ORDER BY
	Monastery



/* Problem 14 - Monasteries by Continents and Countries.

-- 1. Write and execute a SQL command that changes the country named "Myanmar" to its other name "Burma".

-- 2. Add a new monastery holding the following information: Name="Hanga Abbey", Country="Tanzania".

-- 3. Add a new monastery holding the following information: Name="Myin-Tin-Daik", Country="Myanmar".

-- 4. Find the count of monasteries for each continent and not deleted country. Display the continent name, 
   the country name and the count of monasteries. Include also the countries with 0 monasteries. 
   Sort the results by monasteries count (from largest to lowest), then by country name alphabetically. 
   Name the columns exactly like in the table below.
*/
-- 14.1
UPDATE 
	Countries
SET 
	CountryName = 'Burma'
WHERE
	CountryName = 'Myanmar'

-- 14.2
INSERT INTO Monasteries ([Name], CountryCode)
VALUES 
(
	'Hanga Abbey', 
	(SELECT CountryCode FROM Countries WHERE CountryName = 'Tanzania')
)

-- 14.3
INSERT INTO Monasteries ([Name], CountryCode)
VALUES 
(
	'Myin-Tin-Daik', 
	(SELECT CountryCode FROM Countries WHERE CountryName = 'Mianmar')
)

-- 14.4
SELECT 
	cnt.ContinentName,
	c.CountryName,
	COUNT(m.Id) AS MonasteriesCount
FROM
	Monasteries m
RIGHT JOIN
	Countries c ON m.CountryCode = c.CountryCode
JOIN 
	Continents cnt ON c.ContinentCode = cnt.ContinentCode
WHERE
	c.IsDeleted = 0
GROUP BY
	cnt.ContinentName,
	c.CountryName
ORDER BY
	MonasteriesCount DESC,
	c.CountryName

