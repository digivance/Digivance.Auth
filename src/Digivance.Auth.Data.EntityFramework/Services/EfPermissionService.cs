using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
                EntityAccess = command.EntityAccess,
                EntityId = command.EntityId,
                EntityType = command.EntityType,
                ScopeId = command.ScopeId
            };

            context.Permissions.Add(permission);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
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
        public Task<bool> ExistsAsync(Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken)
            => context.Permissions
                .Where(x => x.ScopeId == scopeId)
                .Where(x => x.EntityAccess == entityAccess)
                .Where (x => x.EntityType == entityType)
                .Where(x=> x.EntityId == entityId)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<Permission?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return null;

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public async Task<Permission?> GetAsync(Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.ScopeId == scopeId)
                .Where(x => x.EntityAccess == entityAccess)
                .Where(x => x.EntityType == entityType)
                .Where(x => x.EntityId == entityId)
                .FirstOrDefaultAsync(cancellationToken);

            if (permission == null)
                return null;

            return mapper.Map<Permission>(permission);
        }

        /// <inheritdoc />
        public Task<bool> HasPermissionAsync(ClaimsPrincipal principal, Guid permissionId, CancellationToken cancellationToken)
            => context.Permissions
                .Where(x => x.Id == permissionId)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<bool> HasPermissionAsync(ClaimsPrincipal principal, Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken)
        {
            // If entity id provided, try to check for explicit permission
            if (entityId != null)
            {
                // Get the explicit permission
                var explicitPermission = await context.Permissions
                    .Where(x => x.EntityAccess == entityAccess)
                    .Where(x => x.EntityType == entityType)
                    .Where(x => x.EntityId == entityId)
                    .FirstOrDefaultAsync(cancellationToken);

                // If we found the explicitPermission and the principal has a "permissions" claim
                // with for the explicitPermission.Id, we return true
                if (explicitPermission != null && principal.Claims.Where(x => Guid.Parse(x.Value) == explicitPermission.Id).Any())
                    return true;
            }

            // If entity id not provided, or principal does not have the explicit
            // permission, we check to see if the principal has the null entityId
            // permission in this scope (e.g. global entity type access in this scope).
            var scopePermission = await context.Permissions
                .Where(x => x.EntityAccess == entityAccess)
                .Where(x => x.EntityType == entityType)
                .Where(x => x.EntityId == null)
                .Where(x => x.ScopeId == scopeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (scopePermission != null && principal.Claims.Where(x => Guid.Parse(x.Value) == scopePermission.Id).Any())
                return true;

            // Nope, principal aint got no permission
            return false;
        }

        /// <inheritdoc />
        public async Task<Permission?> UpdateAsync(UpdatePermission command, CancellationToken cancellationToken)
        {
            var permission = await context.Permissions
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);

            permission.Description = command.Description;
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<Permission>(permission);
        }
    }
}
