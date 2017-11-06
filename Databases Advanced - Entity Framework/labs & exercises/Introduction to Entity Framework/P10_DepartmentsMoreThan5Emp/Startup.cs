using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;

namespace P10_DepartmentsMoreThan5Emp
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Departments
                    .Include(d => d.Employees)
                    .Where(d => d.Employees.Count > 5)
                    .OrderBy(d => d.Employees.Count)
                    .ThenBy(d=>d.Name)
                    .ToList()
                    .ForEach(d =>
                    {
                        Console.WriteLine("{0} - {1}", d.Name, d.Manager.FirstName + " " + d.Manager.LastName);
                        d.Employees
                            .OrderBy(e => e.FirstName)
                            .ThenBy(e => e.LastName)
                            .ToList()
                            .ForEach(e => Console.WriteLine("{0} - {1}", e.FirstName + " " + e.LastName, e.JobTitle));
                        Console.WriteLine(new string('-', 10));
                    });
            }
        }
    }
}
