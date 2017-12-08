using Microsoft.EntityFrameworkCore;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class DeleteUserCommand:ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(0, args);

            AuthenticationManager.Authorize();
            User currentUser = AuthenticationManager.GetCurrentUser();

            using (var context = new TeamBuilderContext())
            {
                context.Entry(currentUser).State = EntityState.Modified;
                currentUser.IsDeleted = true;
                context.SaveChanges();
                AuthenticationManager.Logout();
            }

            return $"User {currentUser.Username} was deleted successfully!";
        }
    }
}
