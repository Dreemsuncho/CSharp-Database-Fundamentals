using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class InviteToTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);
            AuthenticationManager.Authorize();

            string teamName = args[0];
            string username = args[1];
            User currentUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsUserExisting(username) ||
                !CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserNotExist);

            if ((!CommandHelper.IsUserCreatorOfTeam(teamName, currentUser) ||
                 !CommandHelper.IsMemberOfTeam(teamName, currentUser.Username)) &&
                  CommandHelper.IsMemberOfTeam(teamName, username))
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);

            using (var context = new TeamBuilderContext())
            {
                User invitedUser = context.Users
                    .FirstOrDefault(u => u.Username == username);
                    
                if (CommandHelper.IsInviteExisting(teamName, invitedUser))
                    throw new InvalidOperationException(Constants.ErrorMessages.InviteIsAlreadySent);

                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);
                var invitation = new Invitation
                {
                    InvitedUser = invitedUser,
                    InvitedUserId = invitedUser.Id,
                    Team = team,
                    TeamId = team.Id,
                    IsActive = !(currentUser.Id == invitedUser.Id)
                };

                invitedUser.ReceivedInvitaions.Add(invitation);
                invitation.InvitedUser = invitedUser;
                invitation.InvitedUserId = invitedUser.Id;

                context.Invitations.Add(invitation);

                context.SaveChanges();
            }

            return $"Team {teamName} invited {username}!";
        }
    }
}
