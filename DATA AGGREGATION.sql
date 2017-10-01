/* Problem 1 - Records’ Count.
-- Import the database and send the total count of records from the one and only table to Mr. Bodrog. Make sure nothing got lost.
*/
Use Gringotts

SELECT COUNT(*) FROM WizzardDeposits


/* Problem 2 - Longest Magic Wand.
-- Select the size of the longest magic wand. Rename the new column appropriately.
*/
SELECT MAX(MagicWandSize) [Longest Magic Wand] FROM WizzardDeposits


/* Problem 3 - Longest Magic Wand per Deposit Groups.
-- For wizards in each deposit group show the longest magic wand. Rename the new column appropriately.
*/
SELECT DepositGroup, MAX(MagicWandSize) [Longest Magic Wand] FROM WizzardDeposits
	GROUP BY DepositGroup


/* Problem 4 - * Smallest Deposit Group per Magic Wand Size.
-- Select the two deposit groups with the lowest average wand size.
*/
SELECT TOP 2 DepositGroup FROM WizzardDeposits
	GROUP BY DepositGroup
	ORDER BY AVG(MagicWandSize)


/* Problem 5 - Deposit Sum.
-- Select all deposit groups and their total deposit sums.
*/
SELECT DepositGroup, SUM(DepositAmount) [Total Sum] FROM WizzardDeposits
	GROUP BY DepositGroup


/* Problem 6 - Deposits Sum for Ollivander Family.
-- Select all deposit groups and their total deposit sums but only for the wizards who have their magic wands crafted by Ollivander family.
*/
SELECT DepositGroup, SUM(DepositAmount) [Total Sum] FROM WizzardDeposits
	WHERE MagicWandCreator = 'Ollivander family'	
	GROUP BY DepositGroup


/* Problem 7 - Deposit Filter.
-- Select all deposit groups and their total deposit sums but only for the wizards who have their magic wands crafted by Ollivander family. Filter total deposit amounts lower than 150000. Order by total deposit amount in descending order.
*/
SELECT DepositGroup, SUM(DepositAmount) [Total Sum] FROM WizzardDeposits
	WHERE MagicWandCreator = 'Ollivander family' 
	GROUP BY DepositGroup
	HAVING SUM(DepositAmount) < 150000
	ORDER BY SUM(DepositAmount) DESC


/* Problem 8 - Deposit Charge.
-- Create a query that selects:
    • Deposit group 
    • Magic wand creator
    • Minimum deposit charge for each group 
   Select the data in ascending ordered by MagicWandCreator and DepositGroup.
*/
SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) [Min Deposit Charge] FROM WizzardDeposits
	GROUP BY DepositGroup, MagicWandCreator


/* Problem 9 - Age Groups.
-- Write down a query that creates 7 different groups based on their age.
   Age groups should be as follows:
    • [0-10]
    • [11-20]
    • [21-30]
    • [31-40]
    • [41-50]
    • [51-60]
    • [61+]
   The query should return
    • Age groups
    • Count of wizards in it
*/
SELECT CASE WHEN Age BETWEEN 0 AND 10 THEN '[0-10]' 
            WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
			WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
			WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
			WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
			WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
			WHEN Age >= 61 THEN '[61+]' END [Age Group], COUNT(*) [Wizzard Count] FROM WizzardDeposits
	GROUP BY CASE WHEN Age BETWEEN 0 AND 10 THEN '[0-10]' 
            WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
			WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
			WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
			WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
			WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
			WHEN Age >= 61 THEN '[61+]' END


/* Problem 10 - First Letter.
-- Write a query that returns all unique wizard first letters of their first names only if they have deposit of type Troll Chest. Order them alphabetically. Use GROUP BY for uniqueness.
*/
SELECT LEFT(FirstName, 1) [First Leter] FROM WizzardDeposits
	WHERE DepositGroup = 'Troll Chest'
	GROUP BY LEFT(FirstName, 1)


/* Problem 11 - Average Interest.
-- Mr. Bodrog is highly interested in profitability. He wants to know the average interest of all deposit groups split by whether the deposit has expired or not. But that’s not all. He wants you to select deposits with start date after 01/01/1985. Order the data descending by Deposit Group and ascending by Expiration Flag.
*/
SELECT DepositGroup, IsDepositExpired, AVG(DepositInterest) [Average Interest] FROM WizzardDeposits
	WHERE DepositStartDate > '01/01/1985'
	GROUP BY DepositGroup, IsDepositExpired
	ORDER BY DepositGroup DESC, IsDepositExpired ASC


/* Problem 12 - * Rich Wizard, Poor Wizard.
-- Mr. Bodrog definitely likes his werewolves more than you. This is your last chance to survive! Give him some data to play his favorite game Rich Wizard, Poor Wizard. The rules are simple: You compare the deposits of every wizard with the wizard after him. If a wizard is the last one in the database, simply ignore it. In the end you have to sum the difference between the deposits.
   At the end your query should return a single value: the SUM of all differences.
*/
SELECT ABS(SUM(hw.DepositAmount - gw.DepositAmount)) [Sum Difference] FROM WizzardDeposits hw
	JOIN WizzardDeposits gw
	ON hw.Id = gw.Id + 1
	

/* Problem 13 - Department Total Salaries.
-- That’s it! You no longer work for Mr. Bodrog. You have decided to find a proper job as an analyst in SoftUni. 
   It’s not a surprise that you will use the SoftUni database. Things get more exciting here!
   Create a query that shows the total sum of salaries for each department. Order by DepartmentID.
   Your query should return: • DepartmentID
   Example:
   |------------------------------|
   | DepartmentID | TotalSalary   |
   |------------------------------|
   | 1            | 241000.00     |
   | …            | …             |
   |------------------------------|
*/
USE SoftUni

SELECT DepartmentID, SUM(Salary) [Total Salary] FROM Employees
	GROUP BY DepartmentID


/* Problem 14 - Employees Minimum Salaries.
-- Select the minimum salary from the employees for departments with ID (2, 5, 7) but only for those hired after 01/01/2000.
*/
SELECT DepartmentID, MIN(Salary) [Minimum Salary] FROM Employees
	WHERE DepartmentID IN (2, 5, 7) AND HireDate > '01/01/2000'
	GROUP BY DepartmentID


/* Problem 15 - Employees Average Salaries.
-- Select all employees who earn more than 30000 into a new table. Then delete all employees who have ManagerID = 42 (in the new table). Then increase the salaries of all employees with DepartmentID=1 by 5000. Finally, select the average salaries in each department.
*/
SELECT * INTO #TempTable FROM Employees
	WHERE Salary > 30000

DELETE FROM #TempTable
	WHERE ManagerID = 42

UPDATE #TempTable
	SET Salary += 5000
	WHERE DepartmentID = 1

SELECT DepartmentID, AVG(Salary) [Average Salary] FROM #TempTable
	GROUP BY DepartmentID


/* Problem 16 - Employees Maximum Salaries.
-- Find the max salary for each department. Filter those, which have max salaries NOT in the range 30000 – 70000.
*/
SELECT DepartmentID, MAX(Salary) [Max Salary] FROM Employees
	GROUP BY DepartmentID
	HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000	


/* Problem 17 - Employees Count Salaries.
-- Count salaries of all employees who don't have a manager.
*/
SELECT COUNT(*) Count FROM Employees
	WHERE ManagerID IS NULL


/* Problem 18 - * 3rd Highest Salary.
-- Find the third highest salary in each department if there is such. 
*/
SELECT DepartmentID, (SELECT DISTINCT Salary FROM Employees
                        WHERE DepartmentID = e.DepartmentID
						ORDER BY Salary DESC OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) [Third Highest Salary] FROM Employees e
	WHERE (SELECT DISTINCT Salary FROM Employees
                        WHERE DepartmentID = e.DepartmentID
						ORDER BY Salary DESC OFFSET 2 ROWS FETCH NEXT 1 ROWS ONLY) IS NOT NULL
	GROUP BY DepartmentID


/* Problem 19 - ** Salary Challenge.
-- Write a query that returns:
    • FirstName
    • LastName
    • DepartmentID
   Select all employees who have salary higher than the average salary of their respective departments. Select only the first 10 rows. Order by DepartmentID.
*/
SELECT TOP 10 FirstName, LastName, DepartmentID FROM Employees e
	WHERE Salary > (SELECT AVG(Salary) FROM Employees WHERE DepartmentID = e.DepartmentID)
	ORDER BY DepartmentID