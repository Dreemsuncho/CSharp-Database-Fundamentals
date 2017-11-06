using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P03_EmployeesFullInformation
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .OrderBy(e => e.EmployeeId)
                    .ToList()
                    .ForEach(e => Console.WriteLine("{0} {1} {2} {3} {4:f2}", e.FirstName, e.LastName, e.MiddleName, e.JobTitle, e.Salary));
            }
        }
    }
}
