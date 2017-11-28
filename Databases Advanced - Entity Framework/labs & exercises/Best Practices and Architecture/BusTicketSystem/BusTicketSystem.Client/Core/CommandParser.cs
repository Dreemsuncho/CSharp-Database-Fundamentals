using System;
using System.Linq;

namespace BusTicketSystem.Client.Core
{
    internal static class CommandParser
    {
        internal static string[] ParseCommand(string command)
        {
            return command
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(arg => arg.Trim())
                .ToArray();
        }
    }
}
