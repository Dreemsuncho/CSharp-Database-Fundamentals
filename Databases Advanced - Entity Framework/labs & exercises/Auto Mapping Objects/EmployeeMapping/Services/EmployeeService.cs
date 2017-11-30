using System;
using System.Linq;
using System.Globalization;
using EmployeeMapping.Data;
using EmployeeMapping.Models;
using AutoMapper;
using EmployeeMapping.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmployeeMapping.Services
{
    public class EmployeeService
    {
        public void AddEmployee(EmployeeContext context, string[] args)
        {
            string firstName = args[0];
            string lastName = args[1];
            decimal salary = decimal.Parse(args[2]);

            context.Employees.Add(new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Salary = salary
            });
            context.SaveChanges();
        }

        public void SetBirthday(EmployeeContext context, string[] args)
        {
            int employeeId = int.Parse(args[0]);
            DateTime employeeBirthday = DateTime.Parse(args[1]/*, "dd/MM/yyyy", CultureInfo.InvariantCulture*/);

            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                employee.Birthday = employeeBirthday;
                context.SaveChanges();
            }
        }

        public void SetAddress(EmployeeContext context, string[] args)
        {
            int employeeId = int.Parse(args[0]);
            string newAddress = string.Join(" ", args.Skip(1));

            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                employee.Address = newAddress;
                context.SaveChanges();
            }
        }

        public void ShowEmployeeInfo(EmployeeContext context, string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                var employeeViewModel = Mapper.Map<EmployeeDto>(employee);
                Console.WriteLine($"ID: {employeeViewModel.Id} - {employeeViewModel.FirstName} " +
                    $"{employeeViewModel.LastName} -  ${employeeViewModel.Salary:f2}");
            }
        }

        internal void ShowEmployeePersonalInfo(EmployeeContext context, string[] args)
        {
            int employeeId = int.Parse(args[0]);

            var employee = context.Employees.Find(employeeId);
            if (employee != null)
            {
                var employeeViewModel = Mapper.Map<EmployeePersonalInfoDto>(employee);
                Console.WriteLine($"ID: {employeeViewModel.Id} - {employeeViewModel.FirstName} {employeeViewModel.LastName} - " +
                    $"${employeeViewModel.Salary:f2}{Environment.NewLine}Birthday: {employeeViewModel.Birthday}{Environment.NewLine}" +
                                  $"Address: {employeeViewModel.Address}");
            }

        }

        internal void ListEmployeesOlderThan(EmployeeContext context, string[] args)
        {
            int age = int.Parse(args[0]);

            var employees = context.Employees
                .Where(e => (DateTime.Now.Year - e.Birthday.Value.Year) > age)
                .Include(e => e.Manager);
            if (employees.Count() > 0)
            {

                var employeesViewModels = Mapper.Map<IList<EmployeePersonalInfoDto>>(employees);
                employeesViewModels.ToList()
                    .ForEach(e =>
                    {
                        string noManager = "[no manager]";
                        Console.WriteLine($"{e.FirstName} {e.LastName} - ${e.Salary:f2} - Manager: {e.Manager?.FullName ?? noManager}");
                    });
            }
            else
            {
                Console.WriteLine($"No Employees Older Than {age} Years!");
            }
        }

        public void SetManager(EmployeeContext context, string[] args)
        {
            int employeeId = int.Parse(args[0]);
            int managerId = int.Parse(args[1]);

            var employee = context.Employees.Find(employeeId);
            var manager = context.Employees.Find(managerId);

            if (manager != null && employee != null)
            {
                employee.Manager = manager;
                employee.ManagerId = manager.Id;
                manager.Employees.Add(employee);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Manager or employee does not exist!");
            }
        }

        public void ShowManagerInfo(EmployeeContext context, string[] args)
        {
            int managerId = int.Parse(args[0]);
            var manager = context.Employees.Find(managerId);
            if (manager != null)
            {
                var managerViewModel = Mapper.Map<ManagerDto>(manager);
                Console.WriteLine($"{managerViewModel.FullName} | Employees: {managerViewModel.Employees.Count()}");
                Console.WriteLine(string.Join(
                    Environment.NewLine + "- ", managerViewModel.Employees.Select(e => $"{e.FirstName} {e.LastName} - ${e.Salary:f2}")));
            }
            else
            {
                Console.WriteLine($"No such manager with id '{managerId}'");
            }
        }
    }
}
