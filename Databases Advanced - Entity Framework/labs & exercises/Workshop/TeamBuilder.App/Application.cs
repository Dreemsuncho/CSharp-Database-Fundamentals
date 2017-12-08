using System;
using TeamBuilder.App.Core;
using TeamBuilder.Data;

namespace TeamBuilder.App
{
    class Application
    {
        static void Main()
        {
            using (var context = new TeamBuilderContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            var engine = new Engine(new CommandDispatcher());
            engine.Run();
        }

        //TODO
        //ShowEvent
        //Shows details for given event.
    }
}
