namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Data;
    using PhotoShare.Models;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Client.Core.Commands.Abstracts;

    public class DeleteUserCommand : Command, ICommand
    {
        // DeleteUser <username>
        public override string Execute(string[] data)
        {
            string userName = data[1];
            using (var context = new PhotoShareContext())
            {
                User user = CheckUserExistence(userName, context);

                if (user.IsDeleted.Value)
                    throw new InvalidOperationException($"User {userName} is already deleted!");

                user.IsDeleted = true;
                context.SaveChanges();

                return $"User {userName} was deleted from the database!";
            }
        }
    }
}
