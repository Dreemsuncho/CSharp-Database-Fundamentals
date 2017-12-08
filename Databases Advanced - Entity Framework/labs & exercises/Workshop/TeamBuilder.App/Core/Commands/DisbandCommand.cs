using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    class DisbandCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);
            AuthenticationManager.Authorize();

            string teamName = args[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);

            if (!CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));

            using (var context = new TeamBuilderContext())
            {
                Team team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                UserTeam userTeams = currentUser.UserTeams.FirstOrDefault(ut => ut.Team.Name == teamName);

                currentUser.CreatedTeams.Remove(team);
                currentUser.UserTeams.Remove(userTeams);
                context.Teams.Remove(team);
                context.Entry(currentUser).State = EntityState.Modified;

                context.SaveChanges();
            }

            return $"{teamName} has disbanded!";
        }
    }
}
