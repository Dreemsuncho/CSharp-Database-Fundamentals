using PhotoShare.Client.Core.Commands.Abstracts;
using PhotoShare.Client.Core.Commands.Contracts;
using PhotoShare.Data;
using PhotoShare.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    class LoginCommand : Command, ICommand
    {
        public override string Execute(string[] data = null)
        {
            string userName = data[0];
            string password = data[1];
            User currentUser = null;

            using (var context = new PhotoShareContext())
            {
                try
                {
                    currentUser = CheckUserExistence(userName, context);
                }
                finally
                {

                    if (currentUser == null ||
                        currentUser.Password != password)
                        throw new ArgumentException("Invalid username or password!");

                    if (currentUser != null && Engine.currentUser != null && Engine.currentUser.Id == currentUser.Id)
                        throw new ArgumentException("You should logout first!");

                    Engine.currentUser = currentUser;
                }
            }

            return $"User {userName} successfully logged in!";
        }

    }
}
