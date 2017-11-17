using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;

namespace P01_BillsPaymentSystem.App
{
    class Startup
    {
        static void Main()
        {
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
                DbInitializer.Seed(context);
            }
        }
    }
}
