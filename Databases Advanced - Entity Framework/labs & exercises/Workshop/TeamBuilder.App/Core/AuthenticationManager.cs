using System;
using TeamBuilder.App.Utilities;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core
{
    public static class AuthenticationManager
    {
        private static User _currentUser;

        public static void Login(User user)
        {
            _currentUser = user;
        }

        public static void Logout()
        {
            Authorize();
            _currentUser = null;
        }

        public static void Authorize()
        {
            if (_currentUser == null)
                throw new InvalidOperationException(Constants.ErrorMessages.LoginFirst);
        }

        public static bool IsAuthenticated()
        {
            return _currentUser != null;
        }

        public static User GetCurrentUser()
        {
            Authorize();
            return _currentUser;
        }
    }
}
