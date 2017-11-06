using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;

namespace P09_Employee147
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                Employee employee = context.Employees
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .FirstOrDefault(e => e.EmployeeId == 147);

                if (employee != null)
                {
                    Console.WriteLine("{0} - {1}", employee.FirstName + " " + employee.LastName, employee.JobTitle);
                    Console.WriteLine(string.Join(Environment.NewLine, employee.EmployeesProjects.Select(ep => ep.Project.Name).OrderBy(epName => epName)));
                }
            }
        }
    }
}
