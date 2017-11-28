namespace PhotoShare.Client.Core
{
    using PhotoShare.Client.Core.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            string command = commandParameters[0];
            string[] commandParams = commandParameters.Skip(1).ToArray();

            string result = string.Empty;

            var loggedUserCommands = new[]
            {
                "AcceptFirend",
                "AddFriend",
                "AddTag",
                "AddTagTo",
                "AddTown",
                "CreateAlbum",
                "DeleteUser",
                "ModifyUser",
                "ShareAlbum",
                "UploadPicture"
            };
            var logoutUserCommands = new[]
            {
                "Login",
                "RegisterUser",
                "PrintFriendList"
            };


            if ((loggedUserCommands.Contains(command) && Engine.currentUser == null) ||
               (logoutUserCommands.Contains(command) && Engine.currentUser != null))
            {
                throw new InvalidOperationException("Invalid credentials!");
            }

            switch (command)
            {
                case "AcceptFirend":
                    result = TryAcceptFriend(command, commandParams);
                    break;
                case "AddFriend":
                    result = TryAddFirend(command, commandParams);
                    break;
                case "AddTag":
                    result = TryAddTag(command, commandParams);
                    break;
                case "AddTagTo":
                    result = TryAddTagTo(command, commandParams);
                    break;
                case "AddTown":
                    result = TryAddTown(command, commandParams);
                    break;
                case "CreateAlbum":
                    result = TryCreateAlbum(command, commandParams);
                    break;
                case "DeleteUser":
                    result = TryDeleteUser(command, commandParams);
                    break;
                case "ModifyUser":
                    result = TryModifyUser(command, commandParams);
                    break;
                case "PrintFriendList":
                    result = TryPrintFriendList(command, commandParams);
                    break;
                case "RegisterUser":
                    result = TryRegisterUser(command, commandParams);
                    break;
                case "Login":
                    result = TryLoginUser(command, commandParams);
                    break;
                case "Logout":
                    result = TryLogoutUser(command,commandParams);
                    break;
                case "ShareAlbum":
                    result = TryShareAlbum(command, commandParams);
                    break;
                case "UploadPicture":
                    result = TryUploadPicture(command, commandParams);
                    break;
                case "Exit":
                    result = Exit();
                    break;
                default:
                    ThrowInvalidCommand(command);
                    break;
            }

            return result;
        }

        private string TryLogoutUser(string command, string[] commandParams)
        {
            if (commandParams.Any())
                ThrowInvalidCommand(command);

            var commandObj = new LogoutCommand();
            return commandObj.Execute();
        }

        private string TryLoginUser(string command, string[] commandParams)
        {
            if (commandParams.Length != 2)
                ThrowInvalidCommand(command);

            var commandObj = new LoginCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryRegisterUser(string command, string[] commandParams)
        {
            if (commandParams.Length != 4)
                ThrowInvalidCommand(command);

            var commandObj = new RegisterUserCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryAddTown(string command, string[] commandParams)
        {
            if (commandParams.Length != 2)
                ThrowInvalidCommand(command);

            var commandObj = new AddTownCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryUploadPicture(string command, string[] commandParams)
        {
            if (commandParams.Length != 3)
                ThrowInvalidCommand(command);

            var commandObj = new UploadPictureCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryShareAlbum(string command, string[] commandParams)
        {
            if (commandParams.Length != 3)
                ThrowInvalidCommand(command);

            var commandObj = new ShareAlbumCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryPrintFriendList(string command, string[] commandParams)
        {
            if (commandParams.Length != 1)
                ThrowInvalidCommand(command);

            var commandObj = new PrintFriendsListCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryAcceptFriend(string command, string[] commandParams)
        {
            if (commandParams.Length != 2)
                ThrowInvalidCommand(command);

            var commandObj = new AcceptFriendCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryAddFirend(string command, string[] commandParams)
        {
            if (commandParams.Length != 2)
                ThrowInvalidCommand(command);

            var commandObj = new AddFriendCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryAddTagTo(string command, string[] commandParams)
        {
            if (commandParams.Length != 2)
                ThrowInvalidCommand(command);

            var commandObj = new AddTagToCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryCreateAlbum(string command, string[] commandParams)
        {
            if (commandParams.Length < 4)
                ThrowInvalidCommand(command);

            var commandObj = new CreateAlbumCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryAddTag(string command, string[] commandParams)
        {
            if (commandParams.Length != 1)
                ThrowInvalidCommand(command);

            var commandObj = new AddTagCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryModifyUser(string command, string[] commandParams)
        {
            if (commandParams.Length != 3)
                ThrowInvalidCommand(command);

            var commandObj = new ModifyUserCommand();
            return commandObj.Execute(commandParams);
        }

        private string TryDeleteUser(string command, string[] commandParams)
        {
            if (commandParams.Length != 1)
                ThrowInvalidCommand(command);

            var commandObj = new DeleteUserCommand();
            return commandObj.Execute(commandParams);
        }

        private string Exit()
        {
            var commandObj = new ExitCommand();
            return commandObj.Execute();
        }

        private void ThrowInvalidCommand(string command) =>
            throw new InvalidOperationException($"Command {command} not valid!");
    }
}
