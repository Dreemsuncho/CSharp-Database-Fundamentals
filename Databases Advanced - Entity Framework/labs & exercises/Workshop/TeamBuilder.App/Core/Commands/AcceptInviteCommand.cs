using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    class AcceptInviteCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);
            string teamName = args[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            CommandHelper.AllowInviteActionsOrThrow(teamName, currentUser);

            Invitation invitation = currentUser.ReceivedInvitaions
                .FirstOrDefault(i => i.Team.Name == teamName);

            using (var context = new TeamBuilderContext())
            {
                invitation.IsActive = false;
                Team team = invitation.Team;

                UserTeam userTeam = new UserTeam
                {
                    User = currentUser,
                    UserId = currentUser.Id,
                    Team = team,
                    TeamId = team.Id
                };

                context.Entry(invitation).State = EntityState.Modified;
                context.Entry(currentUser).State = EntityState.Modified;
                context.Entry(team).State = EntityState.Modified;
                context.Entry(userTeam).State = EntityState.Added;

                team.UserTeams.Add(userTeam);
                currentUser.UserTeams.Add(userTeam);

                context.SaveChanges();
            }

            return $"User {currentUser.Username} joined team {teamName}!";
        }
    }
}
