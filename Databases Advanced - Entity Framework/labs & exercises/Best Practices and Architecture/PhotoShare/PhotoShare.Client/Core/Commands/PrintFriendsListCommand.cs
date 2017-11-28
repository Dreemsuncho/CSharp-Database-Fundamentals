namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Core.Commands.Abstracts;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;
    using System.Text;

    public class PrintFriendsListCommand : Command, ICommand
    {
        // PrintFriendsList <username>
        public override string Execute(string[] date)
        {
            // TODO prints all friends of user with given username.
            string userName = date[0];
            StringBuilder result = new StringBuilder();

            using (var context = new PhotoShareContext())
            {
                User user = CheckUserExistence(userName, context);

                if (user.FriendsAdded.Count == 0)
                {
                    result.AppendLine("No friends for this user. :(");
                }
                else
                {
                    result.AppendLine("Friends:");
                    user.FriendsAdded.ToList()
                        .ForEach(uf => result.AppendLine($"-{uf.Friend.Username}"));
                }
            }

            return result.ToString();
        }
    }
}
