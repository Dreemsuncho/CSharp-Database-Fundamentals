/* Problem 1 - Employee Adresses.
-- Write a query that selects:
    • EmployeeId
    • JobTitle
    • AddressId
    • AddressText
   Return the first 5 rows sorted by AddressId in ascending order.
*/
USE SoftUni

SELECT TOP 5 e.EmployeeID, e.JobTitle, e.AddressID, a.AddressText FROM Employees e
	JOIN Addresses a
	ON e.AddressID = a.AddressID
	ORDER BY e.AddressID


/* Problem 2 - Addresses with Towns.
-- Display address information of all employees in "SoftUni" database. Select first 50 employees.
   The exact format of data is shown below. 
   Order them by FirstName, then by LastName (ascending).
   Hint: Use three-way join.
*/
SELECT TOP 50 e.FirstName, e.LastName, t.Name Town, a.AddressText FROM Employees e
	JOIN Addresses a
	ON e.AddressID = a.AddressID
		JOIN Towns t
		ON a.TownID = t.TownID
	ORDER BY e.FirstName, e.LastName


/* Problem 3 - Sales Employees.
-- Find all employees that are in the "Sales" department. Use "SoftUni" database.
   Order them by EmployeeID
*/
SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name FROM Employees e
	JOIN Departments d
	ON e.DepartmentID = d.DepartmentID
	WHERE d.Name = 'Sales'
	ORDER BY e.EmployeeID


/* Problem 4 - Employee Departments.
--Write a query that selects:
   • EmployeeID
   • FirstName
   • Salary
   • DepartmentName
  Filter only employees with salary higher than 15000. Return the first 5 rows sorted by DepartmentID in ascending order.
*/
SELECT TOP 5 e.EmployeeID, e.FirstName, e.Salary, d.Name DepartmentName FROM Employees e
	JOIN Departments d
	ON e.DepartmentID = d.DepartmentID
	ORDER BY e.DepartmentID


/* Problem 5 - Employees Without Project.
-- Write a query that selects:
    • EmployeeID
    • FirstName
   Filter only employees without a project. Return the first 3 rows sorted by EmployeeID in ascending order.
*/
SELECT TOP 3 e.EmployeeID, e.FirstName FROM (
	SELECT EmployeeID, FirstName FROM Employees
	WHERE EmployeeID NOT IN (SELECT EmployeeID FROM EmployeesProjects)
) e
ORDER BY e.EmployeeID


/* Problem 6 - Employees Hired After.
-- Show all employees that:
   Are hired after 1/1/1999
   Are either in "Sales" or "Finance" department
   Sorted by HireDate (ascending).
*/
SELECT e.FirstName, e.LastName, e.HireDate, d.Name DeptName FROM Employees e
	JOIN Departments d
	ON (e.DepartmentID = d.DepartmentID AND 
	    e.HireDate > '1/1/1999' AND
        d.Name IN ('Sales', 'Finance'))
	ORDER BY e.HireDate


/* Problem 7 - Employees with Projects.
-- Write a query that selects:
    • EmployeeID
    • FirstName
    • ProjectName
   Filter only employees with a project which has started after 13.08.2002 and it is still ongoing (no end date). Return the first 5 rows sorted by EmployeeID in ascending order.
*/
SELECT TOP 5 e.EmployeeID, e.FirstName, p.Name ProjectName FROM Employees e
	JOIN EmployeesProjects ep
	ON e.EmployeeID = ep.EmployeeID
	JOIN Projects p
	ON ep.ProjectID = p.ProjectID
	WHERE p.StartDate >= '8/13/2002' AND p.EndDate IS NULL
	ORDER BY e.EmployeeID


/* Problem 8 - Employee 24.
-- Write a query that selects:
    • EmployeeID
    • FirstName
    • ProjectName
   Filter all the projects of employee with Id 24. If the project has started after 2005 the returned value should be NULL.
*/
SELECT e.EmployeeID,
       e.FirstName, 
       CASE WHEN p.StartDate > '01/01/2005' 
            THEN NULL 
            ELSE p.Name END ProjectName FROM Employees e
	JOIN EmployeesProjects ep
	ON e.EmployeeID = ep.EmployeeID
	JOIN Projects p
	ON ep.ProjectID = p.ProjectID
	WHERE e.EmployeeID = 24


/* Problem 9 - Employee Manager.
-- Write a query that selects:
    • EmployeeID
    • FirstName
    • MangerID
    • ManagerName
   Filter all employees with a manager who has ID equals to 3 or 7. Return all the rows, sorted by EmployeeID in ascending order.
*/
SELECT e.EmployeeID, e.FirstName, e.ManagerID, m.FirstName FROM Employees e
	JOIN Employees m
	ON e.ManagerID = m.EmployeeID
	WHERE e.ManagerID IN (3, 7)
	ORDER BY e.EmployeeID


/* Problem 10 - Employee Summary.
--Display information about employee's manager and employee's department .
  Show only the first 50 employees.
  Sort by EmployeeID (ascending).
*/
SELECT TOP 50 e.EmployeeID,
              CONCAT(e.FirstName, ' ', e.LastName) EmployeeName, 
              CONCAT(m.FirstName, ' ', m.LastName) ManagerName, 
              d.Name FROM Employees e
	JOIN Employees m
	ON e.ManagerID = m.EmployeeID
	JOIN Departments d 
	ON e.DepartmentID = d.DepartmentID
	ORDER BY e.EmployeeID


/* Problem 11 - Min Average Salary.
-- Display lowest average salary of all departments.
   Calculate average salary for each department.
   Then show the value of smallest one.
*/
SELECT MIN(a.AvgSalary) MinAverageSalary FROM (
	SELECT AVG(Salary) AvgSalary FROM Employees
	GROUP BY DepartmentID
) as a


/* Problem 12 - Highest Peaks in Bulgaria.
-- Write a query that selects:
    • CountryCode
    • MountainRange
    • PeakName
    • Elevation
   Filter all peaks in Bulgaria with elevation over 2835. Return all the rows sorted by elevation in descending order.
*/
USE Geography

SELECT mc.CountryCode, m.MountainRange, p.PeakName, p.Elevation FROM Peaks p
	JOIN MountainsCountries mc
	ON p.MountainId = mc.MountainId
	JOIN Mountains m
	ON mc.MountainId = m.Id
	WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
	ORDER BY p.Elevation DESC


/* Problem 13 - Count Mountain Ranges.
-- Write a query that selects:
    • CountryCode
    • MountainRanges
   Filter the count of the mountain ranges in the United States, Russia and Bulgaria.
*/
SELECT mc.CountryCode, COUNT(m.MountainRange) MountainRanges FROM MountainsCountries mc
	JOIN Mountains m
	ON mc.MountainId = m.Id
	GROUP BY mc.CountryCode
	HAVING mc.CountryCode IN ('BG', 'US', 'RU')


/* Problem 14 - Countries with Rivers.
-- Write a query that selects:
    • CountryName
    • RiverName
   Find the first 5 countries with or without rivers in Africa. Sort them by CountryName in ascending order.
*/
SELECT TOP 5 c.CountryName, r.RiverName FROM Countries c
	JOIN Continents co
	ON c.ContinentCode = co.ContinentCode
	LEFT JOIN CountriesRivers cr
	ON c.CountryCode = cr.CountryCode
	LEFT JOIN Rivers r
	ON cr.RiverId = r.Id
	WHERE co.ContinentName = 'Africa'
	ORDER BY c.CountryName


/* Problem 15 - * Continents and Currencies.
-- Write a query that selects:
    • ContinentCode
    • CurrencyCode
    • CurrencyUsage
   Find all continents and their most used currency. Filter any currency that is used in only one country. Sort your results by ContinentCode.
*/
SELECT ContinentCode, CurrencyCode, CurrencyUsage 
FROM
(
	SELECT ContinentCode, 
	       CurrencyCode, 
	       CurrencyUsage,
           DENSE_RANK() OVER (PARTITION BY(ContinentCode) ORDER BY CurrencyUsage DESC) AS [Rank]
	FROM
	(
		SELECT ContinentCode, 
		       CurrencyCode, 
               COUNT(CurrencyCode) AS CurrencyUsage 
		FROM Countries
		GROUP BY ContinentCode,
                 CurrencyCode
	) AS Sub2
) AS Sub1
WHERE Rank = 1 AND CurrencyUsage > 1
ORDER BY ContinentCode


/* Problem 16 - Countries without any Mountains.
-- Write a query that selects CountryCode. Find all the count of all countries, which don’t have a mountain.
    Hint: Result = 231
*/
SELECT COUNT(*) CountryCodes FROM Countries 
	WHERE CountryCode NOT IN (SELECT CountryCode FROM MountainsCountries)


/* Problem 17 - Highest Peak and Longes River by Country.
-- For each country, find the elevation of the highest peak and the length of the longest river, sorted by the highest peak elevation (from highest to lowest), then by the longest river length (from longest to smallest), then by country name (alphabetically). Display NULL when no data is available in some of the columns. Limit only the first 5 rows.
*/
SELECT TOP 5 c.CountryName, 
       MAX(p.Elevation) HighestPeakElevation, 
       MAX(r.Length) LongestRiverLength FROM Countries c
	FULL JOIN MountainsCountries mc
	ON c.CountryCode = mc.CountryCode
	FULL JOIN Peaks p
	ON mc.MountainId = p.MountainId
	JOIN CountriesRivers cr
	ON c.CountryCode = cr.CountryCode
	FULL JOIN Rivers r
	ON cr.RiverId = r.Id
	GROUP BY c.CountryName
	ORDER BY HighestPeakElevation DESC, LongestRiverLength DESC, c.CountryName


/* Problem 18 - * Highest Peak Name and Elevation by Country.
-- For each country, find the name and elevation of the highest peak, along with its mountain. When no peaks are available in some country, display elevation 0, "(no highest peak)" as peak name and "(no mountain)" as mountain name. When multiple peaks in some country have the same elevation, display all of them. Sort the results by country name alphabetically, then by highest peak name alphabetically. Limit only the first 5 rows.
*/
SELECT TOP 5 Country, 
	ISNULL(PeakName, '(no highest peak)') AS [Highest Peak Name],
	ISNULL(Elevation, 0) AS [Highest Peak Elevation],
	ISNULL(MountainRange, '(no mountain)') AS Mountain
FROM 
(
	SELECT c.CountryName AS Country, 
           p.PeakName, 
           MAX(p.Elevation) AS Elevation,
           m.MountainRange,
           DENSE_RANK() OVER(PARTITION BY (c.CountryName) ORDER BY MAX(p.Elevation) DESC) AS Rank
	FROM 
	(
		Countries c
		LEFT JOIN MountainsCountries mc
			ON c.CountryCode = mc.CountryCode
		LEFT JOIN Peaks p
			ON mc.MountainId = p.MountainId
		LEFT JOIN Mountains m
			ON mc.MountainId = m.Id
	)
	GROUP BY c.CountryName, p.PeakName, m.MountainRange
) AS RankedPeaks
WHERE Rank = 1
ORDER BY Country, PeakName
