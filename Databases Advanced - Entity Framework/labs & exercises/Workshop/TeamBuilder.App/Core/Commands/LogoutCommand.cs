using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class LogoutCommand:ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(0, args);
            AuthenticationManager.Authorize();

            User currentUser = AuthenticationManager.GetCurrentUser();
            AuthenticationManager.Logout();

            return $"User {currentUser.Username} successfully logged out!";
        }
    }
}
