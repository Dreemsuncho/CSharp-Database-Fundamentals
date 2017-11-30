using System;
using System.Linq;
using AutoMapper;
using EmployeeMapping.DTOs;
using EmployeeMapping.Models;
using EmployeeMapping.Services;
using EmployeeMapping.Data;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EmployeeMapping
{
    class Startup
    {
        static void Main()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeDto>()
                    .ReverseMap();
                cfg.CreateMap<Employee, EmployeePersonalInfoDto>()
                    .ReverseMap();
                cfg.CreateMap<Employee, ManagerDto>()
                    .ForMember(dest => dest.FullName, options => options.MapFrom(src => src.FirstName + " " + src.LastName))
                    .ReverseMap();
            });

            using (var context = new EmployeeContext())
            {
                Seed(context);
            }

            var employeeService = new EmployeeService();

            Console.WriteLine("Go Go Go!");

            string commandInput;
            while ((commandInput = Console.ReadLine()) != "Exit")
            {
                string[] commandArgs = _ParseInput(commandInput);

                string command = commandArgs[0];
                using (var context = new EmployeeContext())
                {
                    commandArgs = commandArgs.Skip(1).ToArray();
                    switch (command)
                    {
                        case "AddEmployee":
                            employeeService.AddEmployee(context, commandArgs);
                            break;
                        case "SetBirthday":
                            employeeService.SetBirthday(context, commandArgs);
                            break;
                        case "SetAddress":
                            employeeService.SetAddress(context, commandArgs);
                            break;
                        case "EmployeeInfo":
                            employeeService.ShowEmployeeInfo(context, commandArgs);
                            break;
                        case "EmployeePersonalInfo":
                            employeeService.ShowEmployeePersonalInfo(context, commandArgs);
                            break;
                        case "SetManager":
                            employeeService.SetManager(context, commandArgs);
                            break;
                        case "ManagerInfo":
                            employeeService.ShowManagerInfo(context, commandArgs);
                            break;
                        case "ListEmployeesOlderThan":
                            employeeService.ListEmployeesOlderThan(context, commandArgs);
                            break;
                        default:
                            Console.WriteLine("Try again:");
                            break;
                    }
                }
            }
            var currentConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Goodbye :)");
            Console.ForegroundColor = currentConsoleColor;
        }

        private static void Seed(EmployeeContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Employees.Any())
            {
                var employeesAsJsonString = File.ReadAllText("Data/Employees.json");
                var employees =
                    JsonConvert.DeserializeObject<IEnumerable<Employee>>(employeesAsJsonString);
                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
        }

        private static string[] _ParseInput(string commandInput) =>
            commandInput
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim()).ToArray();
    }
}
