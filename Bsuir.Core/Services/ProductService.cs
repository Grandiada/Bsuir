using Bsuir.Core.Models;
using Bsuir.Core.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bsuir.Core.Services
{
    public sealed class ProductService
    {
        private readonly Func<BsuirDbContext> _contextFactory;

        public ProductService(Func<BsuirDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IReadOnlyList<Product>> GetAsync(CancellationToken cancellationToken)
        {

            using (var context = _contextFactory())
            {
                var products = await context.Products.ToListAsync(cancellationToken);

                return products;
            }
        }

        public async Task<Product> AddAsync(Product client, CancellationToken cancellationToken)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (client.Id > 0)
                throw new ArgumentOutOfRangeException(nameof(client));

            using (var context = _contextFactory())
            {
                await context.Products.AddAsync(client, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return client;
            }
        }

        public async Task UpdateAsync(
            Product product, IReadOnlyList<string> propertyNames, CancellationToken cancellationToken)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (product.Id < 1)
                throw new ArgumentOutOfRangeException(nameof(product));
            if (propertyNames == null)
                throw new ArgumentNullException(nameof(propertyNames));
            if (propertyNames.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(propertyNames));

            using (var context = _contextFactory())
            {
                var entity = context.Products.Attach(product);

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
