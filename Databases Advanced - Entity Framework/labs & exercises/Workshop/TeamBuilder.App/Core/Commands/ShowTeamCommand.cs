using System;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(1, args);

            string teamName = args[0];
            if (!CommandHelper.IsTeamExisting(teamName))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));

            using (var context = new TeamBuilderContext())
            {
                var result = new StringBuilder();

                var team = context.Teams
                    .Include(e=>e.UserTeams)
                        .ThenInclude(ut=>ut.User)
                    .FirstOrDefault(t => t.Name == teamName);

                result.AppendLine($"{teamName} {team.Acronym}");
                result.AppendLine($"Members:");

                foreach (UserTeam userTeam in team.UserTeams)
                    result.AppendLine($"--{userTeam.User.Username}");

                return result.ToString();
            }
        }
    }
}
