using BusTicketSystem.Client.Exceptions;
using System;
using System.Linq;

namespace BusTicketSystem.Client.Core
{
    internal class Engine
    {
        private static readonly BusTicketServices _services = new BusTicketServices();

        internal static void Run()
        {

            Console.WriteLine("Enter commands:");
            string commandWithArgs = string.Empty;

            while ((commandWithArgs = Console.ReadLine()) != "exit")
            {
                string[] commandArgs = CommandParser.ParseCommand(commandWithArgs);

                string command = commandArgs[0];
                commandArgs = commandArgs.Skip(1).ToArray();

                string result = string.Empty;

                try
                {
                    switch (command.ToLower())
                    {
                        case "print-info":
                            result = _services.PrintInfo(commandArgs);
                            break;
                        case "buy-ticket":
                            result = _services.BuyTicket(commandArgs);
                            break;
                        case "publish-review":
                            result = _services.PublishReview(commandArgs);
                            break;
                        case "print-reviews":
                            result = _services.PrintReviews(commandArgs);
                            break;
                        case "change-trip-status":
                            result = _services.ChangeTripStatus(commandArgs);
                            break;
                        default:
                            throw new CustomException("Invalid Command");
                    }
                }
                catch (CustomException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }

                Console.WriteLine(result);
            }
        }
    }
}