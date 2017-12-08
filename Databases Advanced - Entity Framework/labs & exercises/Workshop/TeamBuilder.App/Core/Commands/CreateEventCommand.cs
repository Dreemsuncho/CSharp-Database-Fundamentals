using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(4, args);

            string eventName = args[0];
            string description = args[1];
            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(args[2], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) ||
                !DateTime.TryParseExact(args[3], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            else if (startDate > endDate)
                throw new ArgumentException("Start date should be before end date.");

            AuthenticationManager.Authorize();

            using (var context = new TeamBuilderContext())
            {
                var currentUser = AuthenticationManager.GetCurrentUser();
                context.Entry(currentUser).State = EntityState.Modified;
                context.Events.Add(new Event
                {
                    Name = eventName,
                    Description = description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Creator = currentUser,
                    CreatorId = currentUser.Id
                });
                context.SaveChanges();
            }

            return $"Event {eventName} was created successfully!";
        }
    }
}
