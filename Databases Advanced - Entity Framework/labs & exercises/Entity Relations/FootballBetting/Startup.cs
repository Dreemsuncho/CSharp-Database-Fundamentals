using P03_FootballBetting.Data;

namespace P03_FootballBetting
{
    class Startup
    {
        static void Main()
        {
            using (var context = new FootballBettingContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
