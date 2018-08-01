using System;
using Microsoft.EntityFrameworkCore;

namespace Bsuir.Core.Models.Context
{
    public sealed class BsuirDbContext : DbContext
    {
        private readonly string _conntextionString;

        public BsuirDbContext(string conntextionString)
        {
            if (string.IsNullOrEmpty(conntextionString))
                throw new ArgumentOutOfRangeException(nameof(conntextionString));

            _conntextionString = conntextionString;
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_conntextionString);
        }
    }
}
