using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bsuir.Core.Models;
using Bsuir.Core.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace Bsuir.Core.Services
{
    public sealed class ClientService
    {
        private readonly Func<BsuirDbContext> _contextFactory;

        public ClientService(Func<BsuirDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyList<Client>> GetAsync(CancellationToken cancellationToken)
        {

            using (var context = _contextFactory())
            {
                var clients = await context.Clients.ToListAsync(cancellationToken);

                return clients;
            }
        }

        public async Task<Client> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            using (var context = _contextFactory())
            {
                var clients = await context.Clients.Where(i => i.Id == id)
                    .SingleAsync(cancellationToken);

                return clients;
            }
        }

        public async Task<Client> AddAsync(Client client, CancellationToken cancellationToken)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (client.Id > 0)
                throw new ArgumentOutOfRangeException(nameof(client));

            using (var context = _contextFactory())
            {
                await context.Clients.AddAsync(client, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return client;
            }
        }

        public async Task UpdateAsync(
            Client client, IReadOnlyList<string> propertyNames, CancellationToken cancellationToken)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (client.Id < 1)
                throw new ArgumentOutOfRangeException(nameof(client));
            if (propertyNames == null)
                throw new ArgumentNullException(nameof(propertyNames));
            if (propertyNames.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(propertyNames));

            using (var context = _contextFactory())
            {
                var entity = context.Clients.Attach(client);

                foreach (var propertyName in propertyNames)
                {
                    var property = entity.Property(propertyName);

                    if (property == null)
                        throw new InvalidOperationException();

                    property.IsModified = true;
                }

                await context.SaveChangesAsync(cancellationToken);
            }
        }
    }

}
