using System;

namespace BusTicketSystem.Client.Exceptions
{
    class CustomException : Exception
    {
        public CustomException(string message)
            : base(message) { }
    }
}
