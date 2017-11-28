namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AddFriendCommand
    {
        // AddFriend <username1> <username2>
        public string Execute(string[] data)
        {
            string userName1 = data[1];
            string userName2 = data[2];

            using (var context = new PhotoShareContext())
            {
                User user1 = context.Users.FirstOrDefault(u => u.Username == userName1);
                User user2 = context.Users.FirstOrDefault(u => u.Username == userName2);
                if (user1 == null || user2 == null)
                {
                    string nonExistingUser = user1 == null ? userName1 : userName2;
                    throw new ArgumentException($"{nonExistingUser} not found!");
                }

                if (user1.FriendsAdded.Any(u => u.FriendId == user2.Id))
                    throw new InvalidOperationException($"{userName2} is already a friend to {userName1}");

                user1.FriendsAdded.Add(new Friendship
                {
                    User = user1,
                    UserId = user1.Id,
                    Friend = user2,
                    FriendId = user2.Id
                });

                context.SaveChanges();
            }

            return $"Friend {userName2} added to {userName1}";
        }
    }
}
