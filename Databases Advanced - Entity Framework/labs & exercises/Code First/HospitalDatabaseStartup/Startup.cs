using P01_HospitalDatabase.Data;

namespace HospitalDatabaseStartup
{
    class Startup
    {
        static void Main()
        {
            using (var context = new HospitalDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
