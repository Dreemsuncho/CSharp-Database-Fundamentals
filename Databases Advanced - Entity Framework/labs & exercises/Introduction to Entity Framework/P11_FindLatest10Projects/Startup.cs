using P02_DatabaseFirst.Data;
using System;
using System.Linq;

namespace P11_FindLatest10Projects
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Projects
                    .OrderByDescending(p => p.StartDate)
                    .Take(10)
                    .OrderBy(p => p.Name)
                    .ToList()
                    .ForEach(p =>
                    {
                        Console.WriteLine(p.Name);
                        Console.WriteLine(p.Description);
                        Console.WriteLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
                    });
            }
        }
    }
}
