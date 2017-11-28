namespace PhotoShare.Client.Core.Commands
{
    using PhotoShare.Client.Core.Commands.Abstracts;
    using PhotoShare.Client.Core.Commands.Contracts;
    using PhotoShare.Data;
    using PhotoShare.Models;
    using System;
    using System.Linq;

    public class ShareAlbumCommand : Command, ICommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public override string Execute(string[] data)
        {
            int albumId = int.Parse(data[0]);
            string userName = data[1];
            string permission = data[2];

            using (var context = new PhotoShareContext())
            {
                Album album = context.Albums.Find(albumId);
                if (album == null)
                    throw new ArgumentException($"Album {albumId} not found!");

                User user = context.Users.SingleOrDefault(u => u.Username == userName);
                if (user == null)
                    throw new ArgumentException($"User {userName} not found!");

                if (permission != "Owner" || permission != "Viewer")
                    throw new ArgumentException("Permission must be either “Owner” or “Viewer”!");

                user.AlbumRoles.Add(new AlbumRole
                {
                    Album = album,
                    AlbumId = album.Id,
                    User = user,
                    UserId = user.Id,
                    Role = permission == "Owner" 
                        ? Role.Owner 
                        : Role.Viewer
                });
                context.SaveChanges();

                return $"Username {userName} added to album {album.Name} ({permission})";
            }
        }
    }
}
