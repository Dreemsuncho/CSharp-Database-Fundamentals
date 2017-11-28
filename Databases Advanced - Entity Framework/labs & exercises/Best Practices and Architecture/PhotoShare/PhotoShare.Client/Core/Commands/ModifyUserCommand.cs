namespace PhotoShare.Client.Core.Commands
{
    using Microsoft.EntityFrameworkCore;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class ModifyUserCommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            string userName = data[0];
            string property = data[1];
            string newValue = data[2];

            User currentUser;
            using (var context = new PhotoShareContext())
            {
                currentUser = context.Users.SingleOrDefault(u => u.Username == userName);
                if (currentUser == null)
                    throw new ArgumentException($"User {userName} not found!");
            }

            string result = $"User {userName} {property} is {newValue}";

            switch (property)
            {
                case "Password":
                    CheckPassword(newValue);
                    SetPassword(currentUser, newValue);
                    break;
                case "BornTown":
                    Town userBornTown = CheckTownExistence(property, newValue);
                    SetBornTown(currentUser, userBornTown);
                    break;
                case "CurrentTown":
                    Town userCurrentTown = CheckTownExistence(property, newValue);
                    SetCurrentTown(currentUser, userCurrentTown);
                    break;
                default:
                    result = $"Property {property} not supported!";
                    break;
            }

            return result;
        }

        private Town CheckTownExistence(string property, string newValue)
        {
            using (var context = new PhotoShareContext())
            {
                Town town = context.Towns.SingleOrDefault(t => t.Name == newValue);
                if (town == null)
                {
                    string remainMessage = $"Town {newValue} not found!";
                    ThrowValueNotValid(newValue, remainMessage);
                }

                return town;
            }
        }

        private void SetCurrentTown(User currentUser, Town userTown)
        {
            using (var context = new PhotoShareContext())
            {
                currentUser.CurrentTown = userTown;
                context.Users.Update(currentUser);
                context.SaveChanges();
            }
        }

        private void SetBornTown(User currentUser, Town userTown)
        {
            using (var context = new PhotoShareContext())
            {
                currentUser.BornTown = userTown;
                context.Users.Update(currentUser);
                context.SaveChanges();
            }
        }

        private void CheckPassword(string newValue)
        {
            bool passwordIsValid = false;

            for (char letter = 'a'; letter <= 'z'; letter++)
                if (newValue.Contains(letter) &&
                    newValue.Any(ch => char.IsDigit(ch)))
                {
                    passwordIsValid = true;
                    break;
                }

            if (!passwordIsValid)
            {
                string remainMessage = "Invalid Password";
                ThrowValueNotValid(newValue, remainMessage);
            }
        }

        private void SetPassword(User user, string newPassword)
        {
            using (var context = new PhotoShareContext())
            {
                user.Password = newPassword;
                context.Users.Update(user);
                context.SaveChanges();
            }
        }

        private void ThrowValueNotValid(string newValue, string remain) =>
            throw new ArgumentException($"Value {newValue} not valid.{Environment.NewLine}{remain}");
    }
}
