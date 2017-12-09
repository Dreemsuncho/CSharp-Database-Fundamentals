using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

using Newtonsoft.Json;
using AutoMapper;

using Instagraph.Data;
using Instagraph.Models;
using Instagraph.DataProcessor.Utility;
using Instagraph.DataProcessor.DTO;
using Newtonsoft.Json.Linq;

namespace Instagraph.DataProcessor
{
    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<List<Picture>>(jsonString);

            var result = new StringBuilder();

            pictures.ForEach(p =>
            {
                if (DeserializeHelper.IsPictureValid(p, pictures))
                {
                    context.Pictures.Add(p);
                    result.AppendLine($"Successfully imported Picture {p.Path}.");
                }
                else
                {
                    result.AppendLine(Constants.ErrorInvalidData);
                }
            });
            context.SaveChanges();
            return result.ToString();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            var users = JsonConvert.DeserializeObject<List<UserImportDTO>>(jsonString);

            var result = new StringBuilder();

            users.ForEach(u =>
            {
                if (DeserializeHelper.IsUserValid(u, context))
                {
                    var profilePicture = context.Pictures.FirstOrDefault(p => p.Path == u.ProfilePicture);
                    var user = Mapper.Map<User>(u, opt => opt.Items["ProfilePicture"] = profilePicture);
                    context.Users.Add(user);
                    result.AppendLine($"Successfully imported User {user.Username}.");
                }
                else
                {
                    result.AppendLine(Constants.ErrorInvalidData);
                }
            });
            context.SaveChanges();
            return result.ToString();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            var usersFollowers = JsonConvert.DeserializeObject<List<UserFollowerImportDTO>>(jsonString);

            var result = new StringBuilder();

            usersFollowers.ForEach(uf =>
            {
                var user = context.Users.FirstOrDefault(u => u.Username == uf.User);
                var follower = context.Users.FirstOrDefault(u => u.Username == uf.Follower);

                if (DeserializeHelper.IsUserFollowerValid(user, follower, context))
                {
                    var userFollower =
                        Mapper.Map<UserFollower>(uf, opt =>
                        {
                            opt.Items["User"] = user;
                            opt.Items["UserId"] = user.Id;
                            opt.Items["Follower"] = follower;
                            opt.Items["FollowerId"] = follower.Id;
                        });

                    context.UsersFollowers.Add(userFollower);

                    context.SaveChanges();
                    user.Followers.Add(userFollower);
                    follower.UsersFollowing.Add(userFollower);
                    result.AppendLine($"Successfully imported Follower {follower.Username} to User {user.Username}.");
                }
                else
                {
                    result.AppendLine(Constants.ErrorInvalidData);
                }
            });
            context.SaveChanges();
            return result.ToString();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var xmlPosts = XDocument.Parse(xmlString);

            var jArray = new JArray();
            foreach (var item in xmlPosts.Root.Elements())
                jArray.Add(new JObject(
                    new JProperty("caption", item.Element("caption")?.Value),
                    new JProperty("user", item.Element("user")?.Value),
                    new JProperty("picture", item.Element("picture")?.Value)));

            var jsonPosts = JsonConvert.SerializeObject(jArray, Newtonsoft.Json.Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var posts = JsonConvert.DeserializeObject<List<PostImportDTO>>(jsonPosts);

            var result = new StringBuilder();
            posts.ForEach(p =>
            {
                if (DeserializeHelper.IsUserExist(p.User, context) &&
                    DeserializeHelper.IsPictureExist(p.Picture, context))
                {
                    var user = context.Users.First(u => u.Username == p.User);
                    var picture = context.Pictures.First(p2 => p2.Path == p.Picture);
                    var post = Mapper.Map<Post>(p, opt => { opt.Items["User"] = user; opt.Items["Picture"] = picture; });
                    context.Posts.Add(post);
                    result.AppendLine($"Successfully imported Post {p.Caption}.");
                }
                else
                {
                    result.AppendLine(Constants.ErrorInvalidData);
                }
            });
            context.SaveChanges();
            return result.ToString();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var xml = XElement.Parse(xmlString);

            var jArray = new JArray();
            foreach (var item in xml.Elements())
            {
                var content = item.Element("content")?.Value;
                var username = item.Element("user")?.Value;
                var postId = item.Element("post")?.Attribute("id")?.Value;

                jArray.Add(new JObject(
                    new JProperty("content", content),
                    new JProperty("user", username),
                    new JProperty("postId", postId)));
            }

            var commentsJson = JsonConvert.SerializeObject(jArray, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var comments = JsonConvert.DeserializeObject<List<CommentImportDTO>>(commentsJson);
            var result = new StringBuilder();

            comments.ForEach(c =>
            {

                if (DeserializeHelper.IsUserExist(c.User, context) &&
                    DeserializeHelper.IsPostExist(c.PostId, context))
                {
                    var user = context.Users.First(u => u.Username == c.User);
                    var post = context.Posts.Find(int.Parse(c.PostId));

                    var comment = Mapper.Map<Comment>(c, opt =>
                     {
                         opt.Items["User"] = user;
                         opt.Items["UserId"] = user.Id;
                         opt.Items["Post"] = post;
                         opt.Items["PostId"] = post.Id;
                     });
                    context.Comments.Add(comment);
                    result.AppendLine($"Successfully imported Comment {c.Content}.");
                }
                else
                {
                    result.AppendLine(Constants.ErrorInvalidData);
                }
            });
            context.SaveChanges();
            return result.ToString();
        }
    }
}
