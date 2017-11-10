using System;
using P02_DatabaseFirst.Data;
using P02_DatabaseFirst.Data.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace P15_RemoveTowns
{
    class Startup
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                string townName = Console.ReadLine();
                Town town = context.Towns.FirstOrDefault(t => t.Name == townName);
                List<Address> addresses = context.Addresses
                    .Include(a => a.Employees)
                    .Where(a => a.TownId == town.TownId)
                    .ToList();

                addresses.ForEach(a =>
                {
                    a.Employees.ToList().ForEach(e => e.AddressId = null);
                    a.Employees.Clear();
                });

                context.RemoveRange(addresses);
                context.Remove(town);
                context.SaveChanges();

                Console.WriteLine("{0} address in {1} was deleted", addresses.Count(), townName);
                //ss
            }
        }
    }
}
