namespace PhotoShare.Client.Core.Commands
{
    using Models;
    using Data;
    using System.Linq;
    using System;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class AddTownCommand : ICommand
    {
        // AddTown <townName> <countryName>
        public string Execute(string[] data)
        {
            string townName = data[1];
            string country = data[0];

            using (PhotoShareContext context = new PhotoShareContext())
            {
                Town town = context.Towns.SingleOrDefault(t => t.Name == townName);

                if (town != null)
                    throw new ArgumentException($"Town {townName} was already added!");

                town = new Town
                {
                    Name = townName,
                    Country = country
                };

                context.Towns.Add(town);
                context.SaveChanges();

                return $"Town {townName} was added successfully!";
            }
        }
    }
}
