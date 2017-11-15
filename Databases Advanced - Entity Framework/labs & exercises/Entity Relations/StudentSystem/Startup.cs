using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    class Startup
    {
        static void Main()
        {
            using (var context = new StudentSystemContext())
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }
    }
}
