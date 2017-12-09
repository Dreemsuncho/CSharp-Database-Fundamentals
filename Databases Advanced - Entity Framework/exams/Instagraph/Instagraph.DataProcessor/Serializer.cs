using System;

using Instagraph.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Instagraph.DataProcessor.DTO;
using System.Collections.Generic;
using Newtonsoft.Json;
using AutoMapper.QueryableExtensions;
using System.Xml.Linq;

namespace Instagraph.DataProcessor
{
    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Include(p => p.Comments)
                .Include(p => p.User)
                .Include(p => p.Picture)
                .OrderBy(p => p.Id)
                .Where(p => p.Comments.Count == 0)
                .ToList();

            var dtoPosts = Mapper.Map<List<PostExportDTO>>(posts);
            var jsonPosts = JsonConvert.SerializeObject(dtoPosts);
            return jsonPosts;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                .Include(u => u.Followers)
                .Where(u => u.Posts.Any(p => p.Comments.Any(c => u.Followers.Any(f => f.FollowerId == c.UserId))))
                .ToList();

            var dtoUsers = Mapper.Map<List<UserExportDTO>>(users);
            var jsonUsers = JsonConvert.SerializeObject(dtoUsers);

            return jsonUsers;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var dtoUsers = context.Users
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                .ProjectTo<UserCommentsOnPostExport>()
                .OrderByDescending(uDTO => uDTO.MostComments)
                .ThenBy(uDTO => uDTO.Username)
                .ToList();

            var xUsers = new XElement("users");
            dtoUsers.ForEach(u =>
            {
                var xUser = new XElement("user");
                xUser.SetElementValue("Username", u.Username);
                xUser.SetElementValue("MostComments", u.MostComments);
                xUsers.Add(xUser);
            });
            string usersString = xUsers.ToString();
            return usersString;
        }
    }
}
