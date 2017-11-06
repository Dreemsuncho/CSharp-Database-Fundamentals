using System;
using System.Linq;
using P02_DatabaseFirst.Data;

namespace P13_FindEmpFNameStartsWith
{
    class Startup
    {
        static void Main()
        {

            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .Where(e => e.FirstName.StartsWith("Sa"))
                    .OrderBy(e=>e.FirstName)
                    .ThenBy(e=>e.LastName)
                    .ToList()
                    .ForEach(e => Console.WriteLine("{0} {1} - {2} - (${3:f2})", e.FirstName, e.LastName, e.JobTitle, e.Salary));
            }
        }
    }
}
