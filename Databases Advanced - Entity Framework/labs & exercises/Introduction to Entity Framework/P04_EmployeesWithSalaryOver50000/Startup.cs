using System;
using System.Linq;

using P02_DatabaseFirst.Data;

namespace P04_EmployeesWithSalaryOver50000
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .Where(e => e.Salary >= 50000)
                    .Select(e => e.FirstName)
                    .OrderBy(e => e)
                    .ToList()
                    .ForEach(Console.WriteLine);
            }
        }
    }
}
