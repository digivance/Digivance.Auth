using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Entity framework implementation of our IScopeService
    /// </summary>
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
    /// <remarks>
    /// Standard constructor
    /// </remarks>
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
    public class EfScopeService(AuthContext context, EntityMapper mapper) : IScopeService
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
        public async Task<Scope> CreateAsync(CreateScope command, CancellationToken cancellationToken)
        {
            var scope = new ScopeEntity
            {
                Description = command.Description,
                Name = command.Name,
                TenantId = command.TenantId
            };

            context.Scopes.Add(scope);
            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<Scope>(scope);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var scope = await context.Scopes
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (scope != null)
            {
                context.Scopes.Remove(scope);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
            => context.Scopes
                .Where(x => x.Id == id)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid? tenantId, string name, CancellationToken cancellationToken)
            => context.Scopes
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Scope?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var scope = await context.Scopes
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (scope == null)
                return null;

            return mapper.Map<Scope>(scope);
        }

        /// <inheritdoc />
        public async Task<Scope?> GetAsync(Guid? tenantId, string name, CancellationToken cancellationToken)
        {
            var scope = await context.Scopes
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (scope == null)
                return null;

            return mapper.Map<Scope>(scope);
        }

        /// <inheritdoc />
        public async Task<Scope?> UpdateAsync(UpdateScope command, CancellationToken cancellationToken)
        {
            var scope = await context.Scopes
               .Where(x => x.Id == command.Id)
               .FirstOrDefaultAsync(cancellationToken);

            if (scope == null)
                return null;

            scope.Name = command.Name;
            scope.Description = command.Description;
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Scope>(scope);
        }
    }
}
