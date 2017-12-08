using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    class AddToTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);
            AuthenticationManager.Authorize();

            string eventName = args[0];
            string teamName = args[1];

            if (!CommandHelper.IsEventExisting(eventName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            else if (!CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));

            User currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserCreatorOfEvent(eventName, currentUser))
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            else if (CommandHelper.TeamIsMemberOfEvent(eventName, teamName))
                throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);

            using (var context = new TeamBuilderContext())
            {
                Team team = context.Teams
                    .Include(t=>t.UserTeams)
                    .FirstOrDefault(t => t.Name == teamName);
                Event ev = context.Events
                    .Include(e=>e.ParticipatingEventTeams)
                    .FirstOrDefault(e => e.Name == eventName);

                var eventTeam = new EventTeam
                {
                    Team = team,
                    TeamId = team.Id,
                    Event = ev,
                    EventId = ev.Id
                };

                team.Events.Add(eventTeam);
                ev.ParticipatingEventTeams.Add(eventTeam);

                context.Entry(eventTeam).State = EntityState.Added;
                context.SaveChanges();
            }

            return $"Team {teamName} added for {eventName}!";
        }
    }
}
