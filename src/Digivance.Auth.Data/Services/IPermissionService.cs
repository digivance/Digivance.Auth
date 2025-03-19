using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;
using System.Security.Claims;

namespace Digivance.Auth.Data.Services
{
    /// <summary>
    /// Interface representing our Permission service
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Creates a new permission record
        /// </summary>
        /// <param name="command">The create permission command</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The Permission DTO model</returns>
        public Task<Permission> CreateAsync(CreatePermission command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing permission by unique id if it exists, does
        /// nothing if it does not
        /// </summary>
        /// <param name="id">Unique id of the permission record to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await, or don't</returns>
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a permission exists by this unique id
        /// </summary>
        /// <param name="id">Unique id of the permission to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a permission exists by this unique id</returns>
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a permission exists by it's name within a given scope
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to look in</param>
        /// <param name="entityAccess">The access permission to check for (such as "Create", "Read", "Update", "Delete")</param>
        /// <param name="entityId">Unique id of the entity to check for permission to</param>
        /// <param name="entityType">The type of entity to check for permission to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a permission with this name exists within this scope</returns>
        public Task<bool> ExistsAsync(Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Permission DTO model representing the requested permission by id
        /// </summary>
        /// <param name="id">Unique id of the permission to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> GetAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Permission DTO model representing the requested permission by
        /// scope id and name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to get permission from</param>
        /// <param name="entityAccess">The access permission to check for (such as "Create", "Read", "Update", "Delete")</param>
        /// <param name="entityId">Unique id of the entity to check for permission to</param>
        /// <param name="entityType">The type of entity to check for permission to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> GetAsync(Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken);

        /// <summary>
        /// Helper method to ensure the provided principal contains the permissionId claim.
        /// </summary>
        /// <param name="principal">The claims principal to check</param>
        /// <param name="permissionId">Unique id value of the "permissions" claim type</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>boolean</returns>
        public Task<bool> HasPermissionAsync(ClaimsPrincipal principal, Guid permissionId, CancellationToken cancellationToken);

        /// <summary>
        /// Helper method to check if the provided principal contains a permission claim for the requested
        /// entity accesss.
        /// </summary>
        /// <param name="principal">The claims principal to check</param>
        /// <param name="scopeId">Unique id of the scope to get permission name from</param>
        /// <param name="entityAccess">The access permission to check for (such as "Create", "Read", "Update", "Delete")</param>
        /// <param name="entityId">Unique id of the entity to check for permission to</param>
        /// <param name="entityType">The type of entity to check for permission to</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>boolean</returns>
        public Task<bool> HasPermissionAsync(ClaimsPrincipal principal, Guid scopeId, string entityAccess, string entityType, Guid? entityId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing Permission based on the provided unique id and the update command
        /// </summary>
        /// <param name="command">Update command to apply</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> UpdateAsync(UpdatePermission command, CancellationToken cancellationToken);
    }
}
