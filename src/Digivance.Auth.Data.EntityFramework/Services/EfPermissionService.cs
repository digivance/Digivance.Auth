using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Entity framework implementation of our IPermssionService
    /// </summary>
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
    public class EfPermissionService(AuthContext context, EntityMapper mapper) : IPermissionService
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
        public async Task<Permission> CreateAsync(CreatePermission command, CancellationToken cancellationToken)
        {
            var permission = new PermissionEntity
            {
                Description = command.Description,
                Name = command.Name,
                ScopeId = command.ScopeId
            };

            context.Permissions.Add(permission);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission != null)
            {
                context.Permissions.Remove(permission);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
            => context.Permissions
                .Where(x => x.Id == id)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsByNameAsync(Guid? scopeId, string name, CancellationToken cancellationToken)
            => context.Permissions
                .Where(x => x.ScopeId == scopeId)
                .Where(x => x.Name == name)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return null;

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public async Task<Permission?> GetByNameAsync(Guid? scopeId, string name, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.ScopeId == scopeId)
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return null;

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public async Task<Permission?> UpdateAsync(Guid id, UpdatePermission command, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return null;

            permission.Description = command.Description;
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Permission>(permission);
        }
    }
}
