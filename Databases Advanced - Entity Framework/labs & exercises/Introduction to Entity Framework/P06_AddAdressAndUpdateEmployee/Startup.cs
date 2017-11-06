using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Linq;

namespace P06_AddNewAdressAndUpdateEmployee
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                Address nakovNewAdress = new Address { AddressText = "Vitoshka 15", TownId = 4 };
                Employee nakov = context.Employees.FirstOrDefault(e => e.LastName == "Nakov");
                nakov.Address = nakovNewAdress;

                context.SaveChanges();
            }

            using (SoftUniContext context = new SoftUniContext())
            {
                context.Employees
                    .Select(e => new { e.AddressId, e.Address.AddressText })
                    .OrderByDescending(e => e.AddressId)
                    .Take(10)
                    .ToList()
                    .ForEach(e => Console.WriteLine(e.AddressText));
            }
        }
    }
}
