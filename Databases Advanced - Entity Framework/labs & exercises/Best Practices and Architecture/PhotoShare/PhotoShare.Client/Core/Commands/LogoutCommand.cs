using PhotoShare.Client.Core.Commands.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    class LogoutCommand : ICommand
    {
        public string Execute(string[] data = null)
        {
            string result = string.Empty;

            if (Engine.currentUser != null)
            {
                result = $"User {Engine.currentUser.Username} successfully logged out!";
                Engine.currentUser = null;
            }
            else
            {
                result = "You should log in first in order to logout.";
            }

            return result;
        }
    }
}
