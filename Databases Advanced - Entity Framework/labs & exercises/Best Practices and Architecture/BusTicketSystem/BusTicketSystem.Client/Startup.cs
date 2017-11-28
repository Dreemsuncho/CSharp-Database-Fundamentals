using Microsoft.EntityFrameworkCore;
using BusTicketSystem.Data;
using BusTicketSystem.Client.Core;

namespace BusTicketSystem.Client
{
    class Startup
    {
        static void Main()
        {
            try
            {
                InitializeDb();
                Engine.Run();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }

        private static void InitializeDb()
        {
            using (var context = new BusTicketContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                DbInitializer.Seed(context);
            }
        }
    }
}
