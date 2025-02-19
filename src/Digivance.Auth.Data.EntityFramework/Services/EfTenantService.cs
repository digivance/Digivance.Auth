using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Entity framework implementation of Tenant service
    /// </summary>
    public class EfTenantService : ITenantService
    {

        /// <summary>
        /// Internally used AuthContext
        /// </summary>
        private readonly AuthContext context;

        /// <summary>
        /// Internally used EntityMapper
        /// </summary>
        private readonly EntityMapper mapper;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="context">The AuthContext to use</param>
        /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
        public EfTenantService(AuthContext context, EntityMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Tenant> CreateAsync(CreateTenant command, CancellationToken cancellationToken)
        {
            var tenant = new TenantEntity
            {
                Description = command.Description,
                Name = command.Name
            };

            context.Tenants.Add(tenant);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Tenant>(tenant);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
                .Where(x => x.Id == tenantId)
                .FirstOrDefaultAsync(cancellationToken);

            if (tenant != null)
            {
                context.Tenants.Remove(tenant);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
            => context.Tenants
            .Where(x => x.Id == id)
            .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
            => context.Tenants
                .Where (x => x.Name == name)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
              .Where(x => x.Id == tenantId)
              .FirstOrDefaultAsync(cancellationToken);

            if (tenant == null)
                return null;

            return mapper.Map<Tenant>(tenant);
        }

        /// <inheritdoc />
        public async Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
               .Where(x => x.Name == name)
               .FirstOrDefaultAsync(cancellationToken);

            if (tenant == null)
                return null;

            return mapper.Map<Tenant>(tenant);
        }

        /// <inheritdoc />
        public async Task<Tenant> UpdateAsync(Guid id, UpdateTenant command, CancellationToken cancellationToken)
        {
            var tenant = await GetByIdAsync(id, cancellationToken);

            if (tenant == null)
                return null;

            tenant.Name = command.Name;
            tenant.Description = command.Description;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<Tenant>(tenant);
        }
    }
}
