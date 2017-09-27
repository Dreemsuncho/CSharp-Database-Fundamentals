/*
   Examine the Databases.
   Download and get familiar with the SoftUni, Diablo and Geography database schemas and tables. You will use them in the current and following exercises to write queries.
*/ 


-- Part I – Queries for SoftUni Database.
USE SoftUni

/* Problem 1 - Find All Information About Departments.
-- Write a SQL query to find all available information about the Departments. Submit your query statements as Prepare DB & run queries.
*/
SELECT * FROM Departments


/* Problem 2 - Find all Department Names.
-- Write SQL query to find all Department names. Submit your query statements as Prepare DB & run queries.
*/
SELECT Name FROM Departments


/* Problem 3 - Find Salary of Each Employee.
-- Write SQL query to find the first name, last name and salary of each employee. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, LastName, Salary FROM Employees


/* Problem 4 - Find Full Name of Each Employee.
-- Write SQL query to find the first, middle and last name of each employee. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, MiddleName, LastName FROM Employees


/* Problem 5 - Find Email Address of Each Employee.
-- Write a SQL query to find the email address of each employee. (by his first and last name). Consider that the email domain is softuni.bg. Emails should look like “John.Doe@softuni.bg". The produced column should be named "Full Email Address". Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName + '.' + LastName + '@softuni.bg' AS [Full Email Adress] FROM Employees


/* Problem 6 - Find All Different Employee's Salaries. 
-- Write a SQL query to find all different employee’s salaries. Show only the salaries. Submit your query statements as Prepare DB & run queries.
*/
SELECT DISTINCT Salary FROM Employees


/* Problem 7 - Find all Information About Employees.
-- Write a SQL query to find all information about the employees whose job title is “Sales Representative”. Submit your query statements as Prepare DB & run queries.
*/
SELECT * FROM Employees 
	WHERE JobTitle IN ('Sales Representative')

/* Problem 8 - Find Names of All Employees by Salary in Range.
-- Write a SQL query to find the first name, last name and job title of all employees whose salary is in the range [20000, 30000]. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, LastName, JobTitle FROM Employees
	WHERE Salary BETWEEN 20000 AND 30000


/* Problem 9 - Find Names of All Employees.
-- Write a SQL query to find the full name of all employees whose salary is 25000, 14000, 12500 or 23600. Full Name is combination of first, middle and last name (separated with single space) and they should be in one column called “Full Name”. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName + ' ' + MiddleName + ' ' + LastName AS [Full Name] FROM Employees
	WHERE Salary IN(25000, 14000, 12500, 23600)


/* Problem 10 - Find All Employees Without Manager.
-- Write a SQL query to find first and last names about those employees that does not have a manager. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, LastName FROM Employees
	WHERE ManagerID IS NULL


/* Problem 11 - Find All Employees with Salary More Than 50000.
-- Write a SQL query to find first name, last name and salary of those employees who has salary more than 50000. Order them in decreasing order by salary. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, LastName, Salary FROM Employees
	WHERE Salary > 50000
	ORDER BY Salary DESC


/* Problem 12 - Find 5 Best Paid Employees.
-- Write SQL query to find first and last names about 5 best paid Employees ordered descending by their salary. Submit your query statements as Prepare DB & run queries.
*/
SELECT TOP 5 FirstName, LastName FROM Employees
	ORDER BY Salary DESC
	

/* Problem 13 - Find All Employee Except Marketing.
-- Write a SQL query to find the first and last names of all employees whose department ID is different from 4. Submit your query statements as Prepare DB & run queries.
*/
SELECT FirstName, LastName FROM Employees
	WHERE DepartmentID <> 4


/* Problem 14 - Sort Employees Table.
   Write a SQL query to sort all records in the Employees table by the following criteria: 
    • First by salary in decreasing order
    • Then by first name alphabetically
    • Then by last name descending
    • Then by middle name alphabetically
   Submit your query statements as Prepare DB & run queries.
*/
SELECT * FROM Employees
    ORDER BY Salary DESC,
             FirstName,
             LastName DESC,
             MiddleName


/* Problem 15 - Create View Employees with Salaries.
-- Write a SQL query to create a view V_EmployeesSalaries with first name, last name and salary for each employee. Submit your query statements as Run skeleton, run queries & check DB.
*/
CREATE VIEW v_EmployeesSalaries AS
	SELECT FirstName, LastName, Salary FROM Employees


/* Problem 16 - Create View Employees with Job Titles.
-- Write a SQL query to create view V_EmployeeNameJobTitle with full employee name and job title. When middle name is NULL replace it with empty string (‘’). Submit your query statements as Run skeleton, run queries & check DB.
*/
CREATE VIEW v_EmployeeNameJobTitle AS
SELECT FirstName + ' ' + IIF(MiddleName IS NULL, '', MiddleName) + ' ' + LastName AS [Full Name], JobTitle FROM Employees


/* Problem 17 - Distinct Job Titles.
-- Write a SQL query to find all distinct job titles. Submit your query statements as Prepare DB & run queries.
*/
SELECT DISTINCT JobTitle FROM Employees


/* Problem 18 - Find First 10 Started Projects.
-- Write a SQL query to find first 10 started projects. Select all information about them and sort them by start date, then by name. Submit your query statements as Prepare DB & run queries.
*/
SELECT TOP 10 * FROM Projects
	ORDER BY StartDate, Name


/* Problem 19 - Last 7 Hired Employees.
-- Write a SQL query to find last 7 hired employees. Select their first, last name and their hire date. Submit your query statements as Prepare DB & run queries.
*/
SELECT TOP 7 FirstName, LastName, HireDate FROM Employees
	ORDER BY HireDate DESC
	

/* Problem 20 - Increase Salaries.
-- Write a SQL query to increase salaries of all employees that are in the Engineering, Tool Design, Marketing or Information Services department by 12%. Then select Salaries column from the Employees table. After that exercise restore your database to revert those changes. Submit your query statements as Prepare DB & run queries.
*/
UPDATE Employees
	SET Salary *= 1.12
	WHERE Employees.DepartmentID IN(SELECT d.DepartmentId FROM Departments d WHERE Name IN('Engineering', 'Tool Design', 'Marketing', 'Information Services'))

SELECT Salary FROM Employees


-- Part II – Queries for Geography Database.
USE Geography

/* Problem 21 - All Montain Peaks.
-- Display all mountain peaks in alphabetical order. Submit your query statements as Prepare DB & run queries.
*/
SELECT PeakName FROM Peaks
	ORDER BY PeakName


/* Problem 22 - Biggest Countries by Population.
-- Find the 30 biggest countries by population from Europe. Display the country name and population. Sort the results by population (from biggest to smallest), then by country alphabetically. Submit your query statements as Prepare DB & run queries.
*/
SELECT TOP 30 CountryName, Population FROM Countries
    WHERE ContinentCode = 'EU'
    ORDER BY Population DESC,
             CountryName


/* Problem 23 - *Countries and Currency (Euro / Not Euro).
-- Find all countries along with information about their currency. Display the country code, country name and information about its currency: either "Euro" or "Not Euro". Sort the results by country name alphabetically. Submit your query statements as Prepare DB & run queries.
   [*Hint: Use CASE ... WHEN.]
*/
SELECT CountryName, CountryCode, IIF(CurrencyCode = 'EUR', 'Euro', 'Not Euro') AS Currency FROM Countries
	ORDER BY CountryName


-- Part III – Queries for Diablo Database.
USE Diablo

/* Problem 24 - All Diablo Characters.
-- Display all characters in alphabetical order. Submit your query statements as Prepare DB & run queries.
*/
SELECT Name FROM Characters
	ORDER BY Name