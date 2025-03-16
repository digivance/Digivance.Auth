using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.Services
{
    public interface IRoleService
    {
        /// <summary>
        /// Creates a new Role record
        /// </summary>
        /// <param name="command">The create Role command</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The Role DTO model</returns>
        public Task<Role> CreateAsync(CreateRole command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing Role by unique id if it exists, does
        /// nothing if it does not
        /// </summary>
        /// <param name="id">Unique id of the Role record to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await, or don't</returns>
        public Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a Role exists by this unique id
        /// </summary>
        /// <param name="id">Unique id of the Role to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a Role exists by this unique id</returns>
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a Role exists by it's name within a given scope
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to look in</param>
        /// <param name="name">Unique (per Role) name to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a Role with this name exists within this scope</returns>
        public Task<bool> ExistsByNameAsync(Guid scopeId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Role DTO model representing the requested Role by id
        /// </summary>
        /// <param name="id">Unique id of the Role to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Role model or null if not found</returns>
        public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a Role DTO model representing the requested Role by
        /// scope id and name
        /// </summary>
        /// <param name="scopeId">Unique id of the scope to get Role from</param>
        /// <param name="name">Unique (per Role) name of the Role to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Role model or null if not found</returns>
        public Task<Role?> GetByNameAsync(Guid scopeId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a ScopeId through roleId of role
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns scopeId</returns>
        public Task<Guid?> GetScopeId(Guid roleId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing Role based on the provided unique id and the update command
        /// </summary>
        /// <param name="id">Unique id of the Role to update</param>
        /// <param name="command">Update command to apply</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Role model or null if not found</returns>
        public Task<Role?> UpdateAsync(UpdateRole command, CancellationToken cancellationToken);
    }
}
