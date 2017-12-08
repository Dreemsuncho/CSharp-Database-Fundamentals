using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamBuilder.App.Core;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Utilities
{
    public static class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (var context = new TeamBuilderContext())
                return context.Teams.Any(c => c.Name == teamName);
        }
        public static bool IsUserExisting(string username)
        {
            using (var context = new TeamBuilderContext())
                return context.Users.Any(c => c.Username == username);
        }
        public static bool IsInviteExisting(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
                return context.Invitations
                    .Any(i => i.Team.Name == teamName &&
                              i.InvitedUserId == user.Id &&
                              i.IsActive);
        }
        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (var context = new TeamBuilderContext())
                return context.Teams
                    .Include(t => t.UserTeams)
                        .ThenInclude(ut => ut.User)
                    .Single(t => t.Name == teamName)
                    .UserTeams.Any(ut => ut.User.Username == username);
        }
        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (var context = new TeamBuilderContext())
                return context.Users
                    .Include(u => u.CreatedTeams)
                    .FirstOrDefault(u => u.Id == user.Id)
                    .CreatedTeams.Any(t => t.Name == teamName);
        }
        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (var context = new TeamBuilderContext())
                return context.Users
                    .Include(u => u.CreatedEvents)
                    .Single(u => u.Id == user.Id)
                    .CreatedEvents.Any(e => e.Name == eventName);
        }
        public static bool IsEventExisting(string eventName)
        {
            using (var context = new TeamBuilderContext())
                return context.Events.Any(e => e.Name == eventName);
        }
        public static bool TeamIsMemberOfEvent(string eventName, string teamName)
        {
            using (var context = new TeamBuilderContext())
            {
                return context.Events
                    .Include(e => e.ParticipatingEventTeams)
                        .ThenInclude(et => et.Team)
                    .FirstOrDefault(e => e.Name == eventName)
                    .ParticipatingEventTeams.Any(et => et.Team.Name == teamName);
            }
        }
        public static void AllowInviteActionsOrThrow(string teamName, User currentUser)
        {
            AuthenticationManager.Authorize();

            if (!CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            if (!currentUser.ReceivedInvitaions.Any(i => i.Team.Name == teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InviteNotFound, teamName));
        }
    }
}
