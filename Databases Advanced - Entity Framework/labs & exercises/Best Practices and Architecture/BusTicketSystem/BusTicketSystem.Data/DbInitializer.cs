using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using BusTicketSystem.Models;
using BusTicketSystem.Models.Enums;

namespace BusTicketSystem.Data
{
    public static class DbInitializer
    {
        public static void Seed(BusTicketContext context)
        {
            if (!context.Tickets.Any())
            {
                string jsonTowns = File.ReadAllText("../BusTicketSystem.Data/JsonData/Towns.json");
                string jsonBusStations = File.ReadAllText("../BusTicketSystem.Data/JsonData/BusStations.json");
                string jsonCompanies = File.ReadAllText("../BusTicketSystem.Data/JsonData/Companies.json");
                string jsonTrips = File.ReadAllText("../BusTicketSystem.Data/JsonData/Trips.json");
                string jsonCustomers = File.ReadAllText("../BusTicketSystem.Data/JsonData/Customers.json");
                string jsonBankAccounts = File.ReadAllText("../BusTicketSystem.Data/JsonData/BankAccounts.json");
                string jsonTickets = File.ReadAllText("../BusTicketSystem.Data/JsonData/Tickets.json");
                string jsonReviews = File.ReadAllText("../BusTicketSystem.Data/JsonData/Reviews.json");

                var towns = JsonConvert.DeserializeObject<IList<Town>>(jsonTowns);
                var busStations = JsonConvert.DeserializeObject<IList<BusStation>>(jsonBusStations);
                var companies = JsonConvert.DeserializeObject<IList<Company>>(jsonCompanies);
                var trips = JsonConvert.DeserializeObject<IList<Trip>>(jsonTrips);
                var customers = JsonConvert.DeserializeObject<IList<Customer>>(jsonCustomers);
                var bankAccounts = JsonConvert.DeserializeObject<IList<BankAccount>>(jsonBankAccounts);
                var tickets = JsonConvert.DeserializeObject<IList<Ticket>>(jsonTickets);
                var reviews = JsonConvert.DeserializeObject<IList<Review>>(jsonReviews);



                context.Towns.AddRange(towns);
                context.SaveChanges();
                for (int i = 0; i < 1000; i += 7)
                {
                    busStations[i].Town = towns[i];
                    busStations[i].TownId = towns[i].Id;

                    busStations[(i + 1) % 1000].Town = towns[i];
                    busStations[(i + 1) % 1000].TownId = towns[i].Id;

                    busStations[(i + 2) % 1000].Town = towns[i];
                    busStations[(i + 2) % 1000].TownId = towns[i].Id;

                    busStations[(i + 3) % 1000].Town = towns[i];
                    busStations[(i + 3) % 1000].TownId = towns[i].Id;

                    busStations[(i + 4) % 1000].Town = towns[i];
                    busStations[(i + 4) % 1000].TownId = towns[i].Id;

                    busStations[(i + 5) % 1000].Town = towns[i];
                    busStations[(i + 5) % 1000].TownId = towns[i].Id;

                    busStations[(i + 6) % 1000].Town = towns[i];
                    busStations[(i + 6) % 1000].TownId = towns[i].Id;

                    towns[i].BusStations.ToList()
                        .AddRange(new List<BusStation>
                        {
                            busStations[i],
                            busStations[(i + 1) % 1000],
                            busStations[(i + 2) % 1000],
                            busStations[(i + 3) % 1000],
                            busStations[(i + 4) % 1000],
                            busStations[(i + 5) % 1000],
                            busStations[(i + 6) % 1000]
                        });
                }
                context.BusStations.AddRange(busStations);
                context.Companies.AddRange(companies);
                context.SaveChanges();
                for (int i = 0; i < 1000; i += 7)
                {
                    trips[i].OriginBusStation = busStations[i];
                    trips[i].OriginBusStationId = busStations[i].Id;

                    trips[(i + 1) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 1) % 1000].OriginBusStationId = busStations[i].Id;

                    trips[(i + 2) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 2) % 1000].OriginBusStationId = busStations[i].Id;

                    trips[(i + 3) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 3) % 1000].OriginBusStationId = busStations[i].Id;

                    trips[(i + 4) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 4) % 1000].OriginBusStationId = busStations[i].Id;

                    trips[(i + 5) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 5) % 1000].OriginBusStationId = busStations[i].Id;

                    trips[(i + 6) % 1000].OriginBusStation = busStations[i];
                    trips[(i + 6) % 1000].OriginBusStationId = busStations[i].Id;
                    //--
                    trips[i].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[i].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 1) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 1) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 2) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 2) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 3) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 3) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 4) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 4) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 5) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 5) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;

                    trips[(i + 6) % 1000].DestinationBusStation = busStations[(i + 12) % 1000];
                    trips[(i + 6) % 1000].DestinationBusStationId = busStations[(i + 12) % 1000].Id;
                    //--
                    trips[i].BusCompany = companies[i];
                    trips[i].BusCompanyId = companies[i].Id;

                    trips[(i + 1) % 1000].BusCompany = companies[i];
                    trips[(i + 1) % 1000].BusCompanyId = companies[i].Id;

                    trips[(i + 2) % 1000].BusCompany = companies[i];
                    trips[(i + 2) % 1000].BusCompanyId = companies[i].Id;

                    trips[(i + 3) % 1000].BusCompany = companies[i];
                    trips[(i + 3) % 1000].BusCompanyId = companies[i].Id;

                    trips[(i + 4) % 1000].BusCompany = companies[i];
                    trips[(i + 4) % 1000].BusCompanyId = companies[i].Id;

                    trips[(i + 5) % 1000].BusCompany = companies[i];
                    trips[(i + 5) % 1000].BusCompanyId = companies[i].Id;

                    trips[(i + 6) % 1000].BusCompany = companies[i];
                    trips[(i + 6) % 1000].BusCompanyId = companies[i].Id;

                    var tr = new List<Trip>
                        {
                            trips[i],
                            trips[(i + 1) % 1000],
                            trips[(i + 2) % 1000],
                            trips[(i + 3) % 1000],
                            trips[(i + 4) % 1000],
                            trips[(i + 5) % 1000],
                            trips[(i + 6) % 1000]
                        };

                    busStations[i].OriginTrips.ToList().AddRange(tr);
                    busStations[(i + 12) % 1000].DestinationTrips.ToList().AddRange(tr);
                    companies[i].Trips.ToList().AddRange(tr);
                }
                context.Trips.AddRange(trips);
                for (int i = 0; i < 1000; i += 7)
                {
                    customers[i].HomeTown = towns[i];
                    customers[i].HomeTownId = towns[i].Id;

                    customers[(i + 1) % 1000].HomeTown = towns[i];
                    customers[(i + 1) % 1000].HomeTownId = towns[i].Id;

                    customers[(i + 2) % 1000].HomeTown = towns[i];
                    customers[(i + 2) % 1000].HomeTownId = towns[i].Id;

                    customers[(i + 3) % 1000].HomeTown = towns[i];
                    customers[(i + 3) % 1000].HomeTownId = towns[i].Id;

                    customers[(i + 4) % 1000].HomeTown = towns[i];
                    customers[(i + 4) % 1000].HomeTownId = towns[i].Id;

                    customers[(i + 5) % 1000].HomeTown = towns[i];
                    customers[(i + 5) % 1000].HomeTownId = towns[i].Id;

                    customers[(i + 6) % 1000].HomeTown = towns[i];
                    customers[(i + 6) % 1000].HomeTownId = towns[i].Id;

                    towns[i].CustomerHomeTowns.ToList()
                        .AddRange(new List<Customer>
                        {
                            customers[i],
                            customers[(i + 1) % 1000],
                            customers[(i + 2) % 1000],
                            customers[(i + 3) % 1000],
                            customers[(i + 4) % 1000],
                            customers[(i + 5) % 1000],
                            customers[(i + 6) % 1000]
                        });
                }
                context.Customers.AddRange(customers);
                context.SaveChanges();
                for (int i = 0; i < 1000; i++)
                {
                    bankAccounts[i].Customer = customers[i];
                    bankAccounts[i].CustomerId = customers[i].Id;
                }
                context.BankAccounts.AddRange(bankAccounts);
                context.SaveChanges();
                for (int i = 0; i < 1000; i++)
                {
                    customers[i].BankAccount = bankAccounts[i];
                    customers[i].BankAccountId = bankAccounts[i].Id;
                }
                for (int i = 0; i < 1000; i += 7)
                {
                    tickets[i].Customer = customers[i];
                    tickets[i].CustomerId = customers[i].Id;

                    tickets[(i + 1) % 1000].Customer = customers[i];
                    tickets[(i + 1) % 1000].CustomerId = customers[i].Id;

                    tickets[(i + 2) % 1000].Customer = customers[i];
                    tickets[(i + 2) % 1000].CustomerId = customers[i].Id;

                    tickets[(i + 3) % 1000].Customer = customers[i];
                    tickets[(i + 3) % 1000].CustomerId = customers[i].Id;

                    tickets[(i + 4) % 1000].Customer = customers[i];
                    tickets[(i + 4) % 1000].CustomerId = customers[i].Id;

                    tickets[(i + 5) % 1000].Customer = customers[i];
                    tickets[(i + 5) % 1000].CustomerId = customers[i].Id;

                    tickets[(i + 6) % 1000].Customer = customers[i];
                    tickets[(i + 6) % 1000].CustomerId = customers[i].Id;
                    //--
                    tickets[i].Trip = trips[i];
                    tickets[i].TripId = trips[i].Id;

                    tickets[(i + 1) % 1000].Trip = trips[i];
                    tickets[(i + 1) % 1000].TripId = trips[i].Id;

                    tickets[(i + 2) % 1000].Trip = trips[i];
                    tickets[(i + 2) % 1000].TripId = trips[i].Id;

                    tickets[(i + 3) % 1000].Trip = trips[i];
                    tickets[(i + 3) % 1000].TripId = trips[i].Id;

                    tickets[(i + 4) % 1000].Trip = trips[i];
                    tickets[(i + 4) % 1000].TripId = trips[i].Id;

                    tickets[(i + 5) % 1000].Trip = trips[i];
                    tickets[(i + 5) % 1000].TripId = trips[i].Id;

                    tickets[(i + 6) % 1000].Trip = trips[i];
                    tickets[(i + 6) % 1000].TripId = trips[i].Id;
                    //--
                    reviews[i].Customer = customers[i];
                    reviews[i].CustomerId = customers[i].Id;

                    reviews[(i + 1) % 1000].Customer = customers[i];
                    reviews[(i + 1) % 1000].CustomerId = customers[i].Id;

                    reviews[(i + 2) % 1000].Customer = customers[i];
                    reviews[(i + 2) % 1000].CustomerId = customers[i].Id;

                    reviews[(i + 3) % 1000].Customer = customers[i];
                    reviews[(i + 3) % 1000].CustomerId = customers[i].Id;

                    reviews[(i + 4) % 1000].Customer = customers[i];
                    reviews[(i + 4) % 1000].CustomerId = customers[i].Id;

                    reviews[(i + 5) % 1000].Customer = customers[i];
                    reviews[(i + 5) % 1000].CustomerId = customers[i].Id;

                    reviews[(i + 6) % 1000].Customer = customers[i];
                    reviews[(i + 6) % 1000].CustomerId = customers[i].Id;
                    //-
                    reviews[i].Company = companies[i];
                    reviews[i].CompanyId = companies[i].Id;

                    reviews[(i + 1) % 1000].Company = companies[i];
                    reviews[(i + 1) % 1000].CompanyId = companies[i].Id;

                    reviews[(i + 2) % 1000].Company = companies[i];
                    reviews[(i + 2) % 1000].CompanyId = companies[i].Id;

                    reviews[(i + 3) % 1000].Company = companies[i];
                    reviews[(i + 3) % 1000].CompanyId = companies[i].Id;

                    reviews[(i + 4) % 1000].Company = companies[i];
                    reviews[(i + 4) % 1000].CompanyId = companies[i].Id;

                    reviews[(i + 5) % 1000].Company = companies[i];
                    reviews[(i + 5) % 1000].CompanyId = companies[i].Id;

                    reviews[(i + 6) % 1000].Company = companies[i];
                    reviews[(i + 6) % 1000].CompanyId = companies[i].Id;

                    var ticks = new List<Ticket>
                    {
                        tickets[i],
                        tickets[(i + 1) % 1000],
                        tickets[(i + 2) % 1000],
                        tickets[(i + 3) % 1000],
                        tickets[(i + 4) % 1000],
                        tickets[(i + 5) % 1000],
                        tickets[(i + 6) % 1000]
                    };
                    var revs = new List<Review>
                    {
                        reviews[i],
                        reviews[(i + 1) % 1000],
                        reviews[(i + 2) % 1000],
                        reviews[(i + 3) % 1000],
                        reviews[(i + 4) % 1000],
                        reviews[(i + 5) % 1000],
                        reviews[(i + 6) % 1000]
                    };

                    trips[i].Tickets.ToList().AddRange(ticks);

                    companies[i].Reviews.ToList().AddRange(revs);

                    customers[i].Tickets.ToList().AddRange(ticks);
                    customers[i].Reviews.ToList().AddRange(revs);
                }
                context.Tickets.AddRange(tickets);
                context.Reviews.AddRange(reviews);
                context.SaveChanges();

                context.Trips
                    .Include(t => t.Tickets)
                    .Where(t => t.Status == Status.Arrived)
                    .ToList()
                    .ForEach(t =>
                        context.ArrivedTrips.Add(
                            new ArrivedTrip
                            {
                                ArrivalTime = t.ArrivalTime,
                                PassengersCount = t.Tickets.Count(),
                                OriginBusStation = t.OriginBusStation,
                                OriginBusStationId = t.OriginBusStationId,
                                DestinationBusStation = t.DestinationBusStation,
                                DestinationBusStationId = t.DestinationBusStationId
                            }));
                context.SaveChanges();
            }
        }
    }
}
