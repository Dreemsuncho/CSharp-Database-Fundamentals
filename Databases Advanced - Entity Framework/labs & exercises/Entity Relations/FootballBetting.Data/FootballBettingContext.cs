using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext() { }
        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Position>(position =>
            {
                position.ToTable("Positions");
            });

            builder.Entity<User>(user =>
            {
                user.ToTable("Users");
            });

            builder.Entity<Country>(country =>
            {
                country.ToTable("Countries");
            });

            builder.Entity<Color>(color =>
            {
                color.ToTable("Colors");
            });

            builder.Entity<Player>(player =>
            {
                player.ToTable("Players");

                player.HasOne(p => p.Team)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                player.HasOne(p => p.Position)
                    .WithMany(t => t.Players)
                    .HasForeignKey(p => p.PositionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Team>(team =>
            {
                team.ToTable("Teams");

                team.HasOne(t => t.PrimaryKitColor)
                    .WithMany(pkc => pkc.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                team.HasOne(t => t.SecondaryKitColor)
                    .WithMany(pkc => pkc.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                team.HasOne(t => t.Town)
                    .WithMany(t => t.Teams)
                    .HasForeignKey(t => t.TownId)
                    .OnDelete(DeleteBehavior.Restrict);

                team.HasMany(t => t.HomeGames)
                    .WithOne(hg => hg.HomeTeam)
                    .HasForeignKey(hg => hg.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                team.HasMany(t => t.AwayGames)
                    .WithOne(hg => hg.AwayTeam)
                    .HasForeignKey(hg => hg.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Game>(game =>
            {
                game.ToTable("Games");

                game.HasOne(g => g.HomeTeam)
                    .WithMany(ht => ht.HomeGames)
                    .HasForeignKey(g => g.HomeTeamId);

                game.HasOne(g => g.AwayTeam)
                    .WithMany(ht => ht.AwayGames)
                    .HasForeignKey(g => g.AwayTeamId);
            });

            builder.Entity<Town>(town =>
            {
                town.ToTable("Towns");

                town.HasOne(t => t.Country)
                    .WithMany(c => c.Towns)
                    .HasForeignKey(t => t.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Bet>(bet =>
            {
                bet.ToTable("Bets");

                bet.HasOne(b => b.User)
                    .WithMany(u => u.Bets)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                bet.HasOne(b => b.Game)
                    .WithMany(b => b.Bets)
                    .HasForeignKey(b => b.GameId)
                    .OnDelete(DeleteBehavior.Restrict);

                bet.Property(b => b.Prediction)
                    .IsRequired();
            });

            builder.Entity<PlayerStatistic>(playerStatistic =>
            {
                playerStatistic.ToTable("PlayerStatistics");

                playerStatistic.HasKey(ps => new { ps.GameId, ps.PlayerId });

                playerStatistic.HasOne(p => p.Game)
                    .WithMany(g => g.PlayerStatistics)
                    .HasForeignKey(p => p.GameId)
                    .OnDelete(DeleteBehavior.Restrict);

                playerStatistic.HasOne(ps => ps.Player)
                    .WithMany(p => p.PlayerStatistics)
                    .HasForeignKey(ps => ps.PlayerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=FootaballDb;Integrated Security=True");
        }
    }
}
