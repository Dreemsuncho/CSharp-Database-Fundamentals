using System.Collections.Generic;

namespace Instagraph.Models
{
    public class User
    {
        public User()
        {
            Followers = new HashSet<UserFollower>();
            UsersFollowing = new HashSet<UserFollower>();
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Picture ProfilePicture { get; set; }
        public int ProfilePictureId { get; set; }

        public ICollection<UserFollower> Followers { get; set; }
        public ICollection<UserFollower> UsersFollowing { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}