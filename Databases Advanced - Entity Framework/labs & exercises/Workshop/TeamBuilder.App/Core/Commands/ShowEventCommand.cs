using System;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Utilities;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowEventCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            string eventName = args[0];
            if (!CommandHelper.IsEventExisting(eventName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));


            using (var context = new TeamBuilderContext())
            {
                var result = new StringBuilder();

                var ev = context.Events
                    .Include(e => e.ParticipatingEventTeams)
                        .ThenInclude(et=>et.Team)
                    .FirstOrDefault(e => e.Name == eventName);

                result.AppendLine($"{eventName} {ev.StartDate} {ev.EndDate} {ev.Description}");
                result.AppendLine($"Teams:");

                foreach (EventTeam eventTeam in ev.ParticipatingEventTeams)
                    result.AppendLine($"--{eventTeam.Team.Name}");

                return result.ToString();
            }
        }
    }
}
