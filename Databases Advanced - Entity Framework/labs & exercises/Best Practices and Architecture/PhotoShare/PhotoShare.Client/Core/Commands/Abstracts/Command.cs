using System;
using System.Linq;
using PhotoShare.Data;
using PhotoShare.Models;
using PhotoShare.Client.Core.Commands.Contracts;

namespace PhotoShare.Client.Core.Commands.Abstracts
{
    public abstract class Command : ICommand
    {
        public User CheckUserExistence(string userName, PhotoShareContext context)
        {
            var user = context.Users.SingleOrDefault(u => u.Username == userName);
            if (user == null)
                throw new ArgumentException($"User with {userName} was not found!");
            return user;
        }

        public abstract string Execute(string[] data = null);
    }
}
