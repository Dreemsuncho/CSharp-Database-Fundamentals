using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class CreateTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            try
            {
                Check.CheckLength(2, args);
            }
            catch
            {
                Check.CheckLength(3, args);
            }

            string teamName = args[0];
            string teamAcronym = args[1];
            string description = args.Length == 3 ? args[0] : null;

            if (CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamExists, teamName));
            else if (teamAcronym.Length != 3)
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, teamAcronym));
            AuthenticationManager.Authorize();

            _CreateTeam(teamName, teamAcronym, description);
            return $"Team {teamName} successfully created!";
        }

        private void _CreateTeam(string teamName, string teamAcronym, string description)
        {
            User currentUser = AuthenticationManager.GetCurrentUser();
            using (var context = new TeamBuilderContext())
            {
                context.Entry(currentUser).State = EntityState.Modified;
                var team = new Team
                {
                    Name = teamName,
                    Acronym = teamAcronym,
                    Description = description,
                    Creator = currentUser,
                    CreatorId = currentUser.Id
                };
                context.Teams.Add(team);
                context.SaveChanges();
            }
        }
    }
}
