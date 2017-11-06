using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P14_DeleteProjectById
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                Project project = context.Projects.Find(2);
                IEnumerable<EmployeesProject> employeesProjects = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);

                context.RemoveRange(employeesProjects);
                context.Remove(project);
                context.SaveChanges();

                context.Projects.Take(10).ToList().ForEach(p => Console.WriteLine(p.Name));
            }
        }
    }
}
