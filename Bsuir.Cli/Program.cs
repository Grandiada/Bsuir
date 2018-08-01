using System;
using System.Threading;
using Bsuir.Cli.Commands;

namespace Bsuir.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(args));

            Console.WriteLine(string.Join(" ", args));

            switch (args[0])
            {
                case "create-db":
                    CreateDatabaseCommand.RunAsync(CancellationToken.None).Wait();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(args));
            }
        }
    }
}
