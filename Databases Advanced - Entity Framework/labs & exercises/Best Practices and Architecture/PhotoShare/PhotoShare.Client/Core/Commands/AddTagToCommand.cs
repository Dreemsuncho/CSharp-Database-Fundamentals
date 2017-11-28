namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Core.Commands.Contracts;
    using System;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System.Linq;

    public class AddTagToCommand : ICommand
    {
        // AddTagTo <albumName> <tag>
        public string Execute(string[] date)
        {
            string albumName = date[0];
            string tagName = date[1];

            using (var context = new PhotoShareContext())
            {
                Album album = context.Albums.SingleOrDefault(a => a.Name == albumName);
                Tag tag = context.Tags.SingleOrDefault(t => t.Name == tagName);

                if (album == null || tag == null)
                    throw new ArgumentException("Either tag or album do not exist!");

                album.AlbumTags.Add(new AlbumTag { Album = album, AlbumId = album.Id, Tag = tag, TagId = tag.Id });
                context.SaveChanges();
            }

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
