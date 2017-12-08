using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class KickMemberCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            AuthenticationManager.Authorize();
            User currentUser = AuthenticationManager.GetCurrentUser();
            string teamName = args[0];
            string username = args[1];

            if (username == currentUser.Username)
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.CommandNotAllowed, "DisbandTeam"));
            else if (!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser))
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            else if (!CommandHelper.IsUserExisting(username))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            else if (!CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            else if (!CommandHelper.IsMemberOfTeam(teamName, username))
                throw new ArgumentException($"User {username} is not a member in {teamName}!");

            using (var context = new TeamBuilderContext())
            {
                User userToKick = context.Users.FirstOrDefault(u => u.Username == username);
                Team fromTeamToKick = context.Teams.FirstOrDefault(t => t.Name == teamName);

                UserTeam userTeam = context.Teams
                    .Include(t=>t.UserTeams)
                    .FirstOrDefault(t => t.Name == teamName)
                    .UserTeams.FirstOrDefault(ut => ut.Team.Name == teamName);

                userToKick.UserTeams.Remove(userTeam);
                fromTeamToKick.UserTeams.Remove(userTeam);

                context.SaveChanges();
            }

            return $"User {username} was kicked from {teamName}!";
        }
    }
}
