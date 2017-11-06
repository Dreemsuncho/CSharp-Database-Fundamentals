using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P05_EmployeesResearchDevelopment
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .Where(e => e.Department.Name == "Research and Development")
                    .Select(e => new { e.FirstName, e.LastName, DepartmentName = e.Department.Name, e.Salary })
                    .OrderBy(e => e.Salary)
                    .ThenByDescending(e => e.FirstName)
                    .ToList()
                    .ForEach(e => Console.WriteLine("{0} {1} from {2} - ${3:f2}", e.FirstName, e.LastName, e.DepartmentName, e.Salary));
            }
        }
    }
}
