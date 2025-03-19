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
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
    public class EfTenantService(AuthContext context, EntityMapper mapper) : ITenantService
    {
        /// <summary>
        /// Internally used AuthContext
        /// </summary>
        private readonly AuthContext context = context;

        /// <summary>
        /// Internally used EntityMapper
        /// </summary>
        private readonly EntityMapper mapper = mapper;

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
        public async Task DeleteAsync(Guid tenantId, CancellationToken cancellationToken)
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
        public Task<bool> ExistsAsync(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return Task.FromResult(true);
            }
            else
            {
                var exists = context.Tenants
                    .Where(x => x.Id == id)
                    .AnyAsync(cancellationToken);

                return exists;
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
            => context.Tenants
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Tenant?> GetAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
              .Where(x => x.Id == tenantId)
              .FirstOrDefaultAsync(cancellationToken);

            if (tenant == null)
                return null;

            return mapper.Map<Tenant>(tenant);
        }

        /// <inheritdoc />
        public async Task<Tenant?> GetAsync(string name, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
               .Where(x => x.Name == name)
               .FirstOrDefaultAsync(cancellationToken);

            if (tenant == null)
                return null;

            return mapper.Map<Tenant>(tenant);
        }

        /// <inheritdoc />
        public async Task<Tenant> UpdateAsync(UpdateTenant command, CancellationToken cancellationToken)
        {
            var tenant = await context.Tenants
              .Where(x => x.Id == command.Id)
              .FirstOrDefaultAsync(cancellationToken);

            if (tenant == null)
                return null;

            tenant.Name = command.Name;
            tenant.Description = command.Description;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<Tenant>(tenant);
        }
    }

    public static class EfTenantServiceExtensions
    {
        public static async Task SeedDefaultTenantPermissions(this AuthContext context, Guid? tenantId, CancellationToken cancellationToken)
        {
            var scopeExists = await context.Scopes
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Name == "")
                .AnyAsync(cancellationToken);

            if (scopeExists)
                return;

            var systemScope = new ScopeEntity
            {
                Description = "Default system scope for this tenant",
                Name = "",
                TenantId = tenantId,
            };

            context.Scopes.Add(systemScope);
            await context.SaveChangesAsync(cancellationToken);

            var permissions = new PermissionEntity[]
            {
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
