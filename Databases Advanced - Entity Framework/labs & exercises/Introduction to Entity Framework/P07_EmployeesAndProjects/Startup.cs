using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P07_EmployeesAndProjects
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .Include(e => e.EmployeesProjects)
                    .ThenInclude(ep => ep.Project)
                    .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Take(30)
                    .Select(e => new
                    {
                        Name = e.FirstName + " " + e.LastName,
                        Manager = e.Manager.FirstName + " " + e.Manager.LastName,
                        Projects = e.EmployeesProjects.Select(ep => ep.Project)
                    })
                    .ToList()
                    .ForEach(e =>
                    {
                        Console.WriteLine("{0} - Manager: {1}", e.Name, e.Manager);
                        e.Projects
                            .ToList()
                            .ForEach(p =>
                            {
                                string dateFormat = "M/d/yyyy h:mm:ss tt";
                                Console.WriteLine("--{0} - {1} - {2}", p.Name, p.StartDate.ToString(dateFormat), p.EndDate?.ToString(dateFormat) ?? "not finished");
                            });
                    });
            }
        }
    }
}
