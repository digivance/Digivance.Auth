using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.Services
{
    public interface IScopeService
    {
        /// <summary>
        /// Creates a new scope record
        /// </summary>
        /// <param name="command">The create scope command</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The scope DTO model</returns>
        public Task<Scope> CreateAsync(CreateScope command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing scope by unique id if it exists, does
        /// nothing if it does not
        /// </summary>
        /// <param name="id">Unique id of the scope record to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await, or don't</returns>
        public Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a scope exists by this unique id
        /// </summary>
        /// <param name="id">Unique id of the scope to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a scope exists by this unique id</returns>
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if a scope exists by it's name within a given tenant
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant to look in</param>
        /// <param name="name">Unique (per scope) name to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if a scope with this name exists within this tenant</returns>
        public Task<bool> ExistsAsync(Guid? tenantId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a scope DTO model representing the requested scope by id
        /// </summary>
        /// <param name="id">Unique id of the scope to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>scope model or null if not found</returns>
        public Task<Scope?> GetAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a scope DTO model representing the requested scope by
        /// tenant id and name
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant to get scope from</param>
        /// <param name="name">Unique (per scope) name of the scope to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>scope model or null if not found</returns>
        public Task<Scope?> GetAsync(Guid? tenantId, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing scope based on the provided unique id and the update command
        /// </summary>
        /// <param name="id">Unique id of the scope to update</param>
        /// <param name="command">Update command to apply</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>scope model or null if not found</returns>
        public Task<Scope?> UpdateAsync(UpdateScope command, CancellationToken cancellationToken);
    }
}
