using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LoginCommand : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(2, args);

            if (AuthenticationManager.IsAuthenticated())
                throw new InvalidCastException(Constants.ErrorMessages.LogoutFirst);

            string username = args[0];
            string password = args[1];

            User user = _GetUserByCredentials(username, password);

            if (user == null || user.Password != password)
                throw new ArgumentException(Constants.ErrorMessages.UserOrPasswordIsInvalid);

            AuthenticationManager.Login(user);

            return $"User {user.Username} successfully logged in!";
        }

        private static User _GetUserByCredentials(string username, string password)
        {
            using (var context = new TeamBuilderContext())
                return context.Users
                    .Include(u => u.ReceivedInvitaions)
                        .ThenInclude(i => i.Team)
                    .Include(u => u.CreatedTeams)
                    .SingleOrDefault(u => u.Username == username && !u.IsDeleted);
        }
    }
}
