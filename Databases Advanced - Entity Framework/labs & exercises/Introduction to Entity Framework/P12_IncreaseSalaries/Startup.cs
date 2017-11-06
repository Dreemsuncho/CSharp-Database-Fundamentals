using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P12_IncreaseSalaries
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                List<Employee> employees = context.Employees
                    .Where(e => e.Department.Name == "Engineering" ||
                                e.Department.Name == "Tool Design" ||
                                e.Department.Name == "Marketing" ||
                                e.Department.Name == "Information Services")
                    .ToList();



                employees.OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList()
                .ForEach(e =>
                {
                    e.Salary *= new decimal(1.12);
                    Console.WriteLine("{0} {1} (${2:f2})", e.FirstName, e.LastName, e.Salary);
                });

                context.SaveChanges();
            }
        }
    }
}
