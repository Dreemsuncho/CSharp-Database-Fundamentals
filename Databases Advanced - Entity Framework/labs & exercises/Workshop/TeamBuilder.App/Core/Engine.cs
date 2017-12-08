using System;

namespace TeamBuilder.App.Core
{
    public class Engine
    {
        private CommandDispatcher _commandDispatcher;

        public Engine(CommandDispatcher commandDispatcher) =>
            _commandDispatcher = commandDispatcher;

        public void Run()
        {
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    string output = _commandDispatcher.Dispatch(input);
                    Console.WriteLine(output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                }
            }
        }
    }
}
