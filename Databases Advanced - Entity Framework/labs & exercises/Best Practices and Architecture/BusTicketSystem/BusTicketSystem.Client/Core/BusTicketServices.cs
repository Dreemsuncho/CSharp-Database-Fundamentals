using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

using BusTicketSystem.Data;
using BusTicketSystem.Models;
using BusTicketSystem.Client.Exceptions;
using BusTicketSystem.Models.Enums;

namespace BusTicketSystem.Client.Core
{
    internal class BusTicketServices
    {
        internal string PrintInfo(string[] args)
        {
            int busStationId = int.Parse(args[0]);

            using (var context = new BusTicketContext())
            {
                var busStation = context.BusStations
                    .AsNoTracking()
                    .Where(bs => bs.Id == busStationId)
                    .Include(bs => bs.Town)
                    .Include(bs => bs.OriginTrips)
                    .Include(bs => bs.DestinationTrips)
                    .SingleOrDefault();

                var result = new StringBuilder();

                result.AppendLine($"{busStation.Name}, {busStation.Town.Name}");

                result.AppendLine($"Arrivals:");
                busStation.OriginTrips
                    .ToList()
                    .ForEach(ot =>
                        result.AppendLine($"From: {ot.OriginBusStation.Town.Name} | " +
                                          $"Arrive at: {ot.ArrivalTime.TimeOfDay.ToString(@"hh\:mm")} | " +
                                          $"Status: {ot.Status.ToString()}"));

                result.AppendLine($"Departures:");
                busStation.DestinationTrips
                    .ToList()
                    .ForEach(ot =>
                        result.AppendLine($"To: {ot.DestinationBusStation.Town.Name} | " +
                                          $"Depart at: {ot.ArrivalTime.TimeOfDay.ToString(@"hh\:mm")} | " +
                                          $"Status: {ot.Status.ToString()}"));

                return result.ToString();
            }
        }

        internal string BuyTicket(string[] args)
        {
            int customerId = int.Parse(args[0]);
            int tripId = int.Parse(args[1]);
            decimal price = decimal.Parse(args[2]);
            int seat = int.Parse(string.Join("", args[3].Skip(1)));

            if (price < 0)
                throw new CustomException("Invalid Price");

            using (var context = new BusTicketContext())
            {
                var customer = context.Customers
                    .Include(c => c.BankAccount)
                    .SingleOrDefault(c => c.Id == customerId);

                if (customer.BankAccount.Balance < price)
                    throw new CustomException($"Isufficient amount of money for customer " +
                        $"{customer.FullName} with bank account number " +
                        $"{customer.BankAccount.AccountNumber}");

                var trip = context.Trips.Find(tripId);
                var ticket = new Ticket
                {
                    Customer = customer,
                    CustomerId = customer.Id,
                    Trip = trip,
                    TripId = trip.Id,
                    Price = price,
                    Seat = seat
                };

                context.Tickets.Add(ticket);
                customer.BankAccount.Balance -= price;
                customer.Tickets.ToList().Add(ticket);

                context.SaveChanges();

                return $"Customer {customer.FullName} bought ticket for trip {tripId} for {price:c} on seat {seat}";
            }
        }

        internal string PublishReview(string[] args)
        {
            int customerId = int.Parse(args[0]);
            double grade = double.Parse(args[1]);
            string busCompanyName = args[2];
            string content = args[3];

            using (var context = new BusTicketContext())
            {
                var customer = context.Customers
                    .Include(c => c.Reviews)
                    .SingleOrDefault(c => c.Id == customerId);

                var company = context.Companies
                    .FirstOrDefault(c => c.Name == busCompanyName);

                if (company == null)
                    throw new CustomException($"No such company '{busCompanyName}'");

                var review = new Review
                {
                    Customer = customer,
                    CustomerId = customer.Id,
                    Company = company,
                    CompanyId = company.Id,
                    Content = content,
                    Grade = grade
                };

                context.Reviews.Add(review);
                context.SaveChanges();
                return $"Customer {customer.FullName} published review for company {busCompanyName}";
            }
        }

        internal string PrintReviews(string[] args)
        {
            int busCompanyId = int.Parse(args[0]);

            using (var context = new BusTicketContext())
            {
                var company = context.Companies
                    .AsNoTracking()
                    .Include(c => c.Reviews)
                        .ThenInclude(r => r.Customer)
                    .SingleOrDefault(c => c.Id == busCompanyId);

                if (company == null)
                    throw new CustomException($"No such company with id '{busCompanyId}'");

                var result = new StringBuilder();

                company.Reviews
                    .ToList()
                    .ForEach(r =>
                        result.AppendLine($"{r.Id} {r.Grade} {r.PublishDate}{Environment.NewLine}" +
                                          $"{r.Customer.FullName}{Environment.NewLine}" +
                                          $"{r.Content}{Environment.NewLine}"));

                return result.ToString();
            }
        }

        internal string ChangeTripStatus(string[] args)
        {
            int tripId = int.Parse(args[0]);
            string newStatus = args[1];

            using (var context = new BusTicketContext())
            {
                var trip = context.Trips
                    .Include(t => t.OriginBusStation)
                        .ThenInclude(obs => obs.Town)
                    .Include(t => t.DestinationBusStation)
                        .ThenInclude(dbs => dbs.Town)
                    .Include(t => t.Tickets)
                    .SingleOrDefault(t => t.Id == tripId);

                if (trip == null)
                    throw new CustomException($"Trip with Id '{tripId}' does not exist!");
                if (trip.Status == Status.Arrived)
                    throw new CustomException("This trip has already arrived!");

                string result = $"Trip from {trip.OriginBusStation.Town.Name} to {trip.DestinationBusStation.Town.Name} " +
                                $"on {trip.DepartureTime}{Environment.NewLine}" +
                                $"Status changed from {trip.Status} to {newStatus}";

                trip.Status = Enum.Parse<Status>(newStatus);


                if (trip.Status == Status.Arrived)
                {
                    var arrivedTrip = new ArrivedTrip
                    {
                        ArrivalTime = trip.ArrivalTime,
                        PassengersCount = trip.Tickets.Count(),
                        OriginBusStation = trip.OriginBusStation,
                        OriginBusStationId = trip.OriginBusStationId,
                        DestinationBusStation = trip.DestinationBusStation,
                        DestinationBusStationId = trip.DestinationBusStationId
                    };
                    context.ArrivedTrips.Add(arrivedTrip);
                    context.SaveChanges();

                    result += $"{Environment.NewLine}On {DateTime.Now} - {arrivedTrip.PassengersCount} passengers arrived at " +
                              $"{arrivedTrip.DestinationBusStation.Town.Name} from {arrivedTrip.DestinationBusStation.Town.Name}";
                }
                return result;
            }
        }
    }
}
//migrationBuilder.Sql("ALTER TABLE Reviews ADD CHECK (Grade >=1 AND Grade <=10)");
