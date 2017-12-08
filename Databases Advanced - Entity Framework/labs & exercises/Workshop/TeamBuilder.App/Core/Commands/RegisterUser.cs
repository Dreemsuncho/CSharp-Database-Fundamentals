using System;
using System.Linq;
using TeamBuilder.App.Core.Commands.Contracts;
using TeamBuilder.App.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    public class RegisterUser : ICommand
    {
        public string Execute(string[] args)
        {
            Check.CheckLength(7, args);

            string username = args[0];
            if (username.Length < Constants.MinUsernameLength || username.Length > Constants.MaxUsernameLength)
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UsernameNotValid, username));

            string password = args[1];
            if (!password.Any(char.IsDigit) || !password.Any(char.IsUpper))
                throw new ArgumentException(string.Format(Constants.ErrorMessages.PasswordNotValid, password));

            string repeatedPassword = args[2];
            if (password != repeatedPassword)
                throw new ArgumentException(Constants.ErrorMessages.PasswordDoesNotMatch);

            string firstName = args[3];
            string lastName = args[4];
            int age;
            Gender gender;

            if (!int.TryParse(args[5], out age) || age <= 0)
                throw new ArgumentException(Constants.ErrorMessages.AgeNotValid);
            if (!Enum.TryParse(args[6], out gender))
                throw new ArgumentException(Constants.ErrorMessages.GenderNotValid);
            if (CommandHelper.IsUserExisting(username))
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UsernameIsTaken, username));

            _RegisterUser(username, password, firstName, lastName, age, gender);
            return $"Username {username} was registered successfully!";
        }

        private void _RegisterUser(string username, string password, string firstName, string lastName, int age, Gender gender)
        {
            using (var context = new TeamBuilderContext())
            {
                context.Users.Add(new User
                {
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                });
                context.SaveChanges();
            }
        }
    }
}
