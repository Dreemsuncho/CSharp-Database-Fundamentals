using System;
using P02_DatabaseFirst.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace P08_AdressesByTown
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                context.Addresses
                    .Include(a => a.Employees)
                    .Include(a=>a.Town)
                    .OrderByDescending(a => a.Employees.Count)
                    .ThenBy(a => a.Town.Name)
                    .ThenBy(a => a.AddressText)
                    .Take(10)
                    .ToList()
                    .ForEach(a => Console.WriteLine("{0}, {1} - {2} employees", a.AddressText, a.Town.Name, a.Employees.Count));
            }
        }
    }
}
