using P03_SalesDatabase.Data;

namespace HospitalDatabaseStartup
{
    class Startup
    {
        static void Main()
        {
            using (var context = new SalesDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
