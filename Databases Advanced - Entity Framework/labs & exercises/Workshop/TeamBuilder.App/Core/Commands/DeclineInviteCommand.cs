using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    class DeclineInviteCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);
            string teamName = args[0];
            User currentUser = AuthenticationManager.GetCurrentUser();
            CommandHelper.AllowInviteActionsOrThrow(teamName, currentUser);

            using (var context = new TeamBuilderContext())
            {
                Invitation invitation = context.Invitations.FirstOrDefault(i => i.Team.Name == teamName);
                currentUser.ReceivedInvitaions.Remove(invitation);
                context.Invitations.Remove(invitation);
                context.SaveChanges();
            }
            return $"Invite from {teamName} declined.";
        }
    }
}
