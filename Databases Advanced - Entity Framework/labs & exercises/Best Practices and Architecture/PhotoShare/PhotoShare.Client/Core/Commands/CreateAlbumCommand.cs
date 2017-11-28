namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using PhotoShare.Client.Core.Commands.Abstracts;
    using PhotoShare.Client.Core.Commands.Contracts;
    using System.Collections.Generic;

    public class CreateAlbumCommand : Command, ICommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public override string Execute(string[] data)
        {
            string userName = data[0];
            string albumTitle = data[1];
            string bgColor = data[2];
            string[] tagNames = data.Skip(3).ToArray();

            using (var context = new PhotoShareContext())
            {
                User user = CheckUserExistence(userName, context);

                Album album = context.Albums.SingleOrDefault(a => a.Name == albumTitle);
                if (album != null)
                    throw new ArgumentException($"Album {albumTitle} exist!");

                Array colors = Enum.GetValues(typeof(Color));
                bool colorIsPresent = false;
                foreach (var c in colors)
                {
                    if (bgColor == c.ToString())
                    {
                        colorIsPresent = true;
                        break;
                    }
                }

                if (!colorIsPresent)
                    throw new ArgumentException($"Color {bgColor} not found!");

                IEnumerable<Tag> tags = context.Tags
                    .Where(t => tagNames.Contains(t.Name))
                    .ToList();

                if (tags.Count() != tagNames.Count())
                    throw new ArgumentException("Invalid tags!");

                album = CreateAlbum(user, albumTitle, tags);
                context.Albums.Add(album);
                context.SaveChanges();
            }
            return null;
        }

        private Album CreateAlbum(User user, string albumTitle, IEnumerable<Tag> tags)
        {
            var albumRoles = new List<AlbumRole> { new AlbumRole { Role = Role.Owner, User = user } };

            var albumTags = new List<AlbumTag>();
            foreach (var tag in tags)
                albumTags.Add(new AlbumTag { Tag = tag, TagId = tag.Id });

            return new Album
            {
                Name = albumTitle,
                AlbumRoles = albumRoles,
                AlbumTags = albumTags
            };
        }
    }
}
