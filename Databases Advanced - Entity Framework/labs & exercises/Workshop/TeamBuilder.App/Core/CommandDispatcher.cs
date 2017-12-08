using System;
using System.IO;
using System.Linq;
using TeamBuilder.App.Core.Commands;

namespace TeamBuilder.App.Core
{
    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;

            string[] commandArgs = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string command = commandArgs.Length > 0 ? commandArgs[0] : string.Empty;
            commandArgs = commandArgs.Skip(1).ToArray();

            switch (command)
            {
                case "Login":
                    result = new LoginCommand().Execute(commandArgs);
                    break;
                case "Logout":
                    result = new LogoutCommand().Execute(commandArgs);
                    break;
                case "RegisterUser":
                    result = new RegisterUser().Execute(commandArgs);
                    break;
                case "DeleteUser":
                    result = new DeleteUserCommand().Execute(commandArgs);
                    break;
                case "CreateEvent":
                    commandArgs = new[] { commandArgs[0], commandArgs[1], $"{commandArgs[2]} {commandArgs[3]}", $"{commandArgs[4]} {commandArgs[5]}" };
                    result = new CreateEventCommand().Execute(commandArgs);
                    break;
                case "CreateTeam":
                    result = new CreateTeamCommand().Execute(commandArgs);
                    break;
                case "InviteToTeam":
                    result = new InviteToTeamCommand().Execute(commandArgs);
                    break;
                case "AcceptInvite":
                    result = new AcceptInviteCommand().Execute(commandArgs);
                    break;
                case "DeclineInvite":
                    result = new DeclineInviteCommand().Execute(commandArgs);
                    break;
                case "KickMember":
                    result = new KickMemberCommand().Execute(commandArgs);
                    break;
                case "Disband":
                    result = new DisbandCommand().Execute(commandArgs);
                    break;
                case "AddTeamTo":
                    result = new AddToTeamCommand().Execute(commandArgs);
                    break;
                case "ShowTeam":
                    result = new ShowTeamCommand().Execute(commandArgs);
                    break;
                case "ShowEvent":
                    result = new ShowEventCommand().Execute(commandArgs);
                    break;
                case "Exit":
                    result = new ExitCommand().Execute(commandArgs);
                    break;
                default:
                    throw new NotSupportedException($"Command {command} not supported!");
            }

            return result;
        }
    }
}
