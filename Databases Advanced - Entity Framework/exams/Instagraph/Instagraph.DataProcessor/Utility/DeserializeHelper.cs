using System.Linq;
using System.Collections.Generic;
using Instagraph.Models;
using Instagraph.Data;
using Instagraph.DataProcessor.DTO;
using System;
using Microsoft.EntityFrameworkCore;

namespace Instagraph.DataProcessor.Utility
{
    internal static class DeserializeHelper
    {
        internal static bool IsPictureValid(Picture p, List<Picture> pictures)
        {
            return p.Path != null &&
                   p.Path.Length > 0 &&
                   pictures.Count(pp => pp.Path == p.Path) == 1 &&
                   p.Size > 0;
        }

        internal static bool IsUserValid(UserImportDTO u, InstagraphContext context)
        {
            return u.Username?.Length <= 30 &&
                   u.Password?.Length <= 20 &&
                   context.Pictures.Any(p => p.Path == u.ProfilePicture);
        }

        internal static bool IsUserFollowerValid(User user, User follower, InstagraphContext context)
        {
            return user != null && follower != null &&
                   (context.UsersFollowers
                        .FirstOrDefault(uf2 => uf2.UserId == user.Id && uf2.FollowerId == follower.Id) == null);
        }

        internal static bool IsUserExist(string username, InstagraphContext context)
        {
            return context.Users.FirstOrDefault(u => u.Username == username) != null;
        }

        internal static bool IsPictureExist(string path, InstagraphContext context)
        {
            return context.Pictures.FirstOrDefault(p => p.Path == path) != null;
        }

        internal static bool IsPostExist(string postId, InstagraphContext context)
        {
            return postId != null && context.Posts.Find(int.Parse(postId)) !=null;
        }
    }
}
