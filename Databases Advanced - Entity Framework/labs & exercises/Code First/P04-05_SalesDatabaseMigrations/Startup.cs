using P04_05_SalesDatabaseMigrations.Data;

namespace P04_05_SalesDatabaseMigrations
{
    class Startup
    {
        static void Main()
        {
           new SalesDbContext().Database.EnsureCreated();
        }
    }
}
