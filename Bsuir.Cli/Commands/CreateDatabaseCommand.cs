using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bsuir.Core.Models.Context;

namespace Bsuir.Cli.Commands
{
    public static class CreateDatabaseCommand
    {
        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Create database command");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = config.GetConnectionString("DefaultConnection");
            var context = new BsuirDbContext(connection);
            Console.WriteLine($"Connection string: {connection} \n Type 'yes' to continue");

            if (Console.ReadLine() != "yes")
                return;

            await context.Database.EnsureDeletedAsync(cancellationToken);

            var success = await context.Database.EnsureCreatedAsync(cancellationToken);

            if (!success)
                throw new InvalidOperationException();

            Console.WriteLine("SUCCESS");
        }
    }
}
