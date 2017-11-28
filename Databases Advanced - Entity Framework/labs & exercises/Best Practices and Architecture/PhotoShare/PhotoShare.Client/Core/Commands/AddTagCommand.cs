namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Utilities;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class AddTagCommand : ICommand
    {
        // AddTag <tag>
        public string Execute(string[] data)
        {
            string tagName = data[0].ValidateOrTransform();

            using (PhotoShareContext context = new PhotoShareContext())
            {
                Tag tag = context.Tags.SingleOrDefault(t => t.Name == tagName);

                if (tag != null)
                    throw new ArgumentException($"Tag {tagName} exist!");

                context.Tags.Add(new Tag
                {
                    Name = tagName
                });

                context.SaveChanges();
            }

            return tagName + " was added successfully to database!";
        }
    }
}
