using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;

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
        public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

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
        /// <param name="name">Unique (per scope) name to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a permission with this name exists within this scope</returns>
        public Task<bool> ExistsByNameAsync(Guid scopeId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Permission DTO model representing the requested permission by id
        /// </summary>
        /// <param name="id">Unique id of the permission to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Permission DTO model representing the requested permission by
        /// scope id and name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to get permission from</param>
        /// <param name="name">Unique (per scope) name of the permission to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> GetByNameAsync(Guid scopeId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing Permission based on the provided unique id and the update command
        /// </summary>
        /// <param name="id">Unique id of the permission to update</param>
        /// <param name="command">Update command to apply</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Permission model or null if not found</returns>
        public Task<Permission?> UpdateAsync(Guid id, UpdatePermission command, CancellationToken cancellationToken);
    }
}
