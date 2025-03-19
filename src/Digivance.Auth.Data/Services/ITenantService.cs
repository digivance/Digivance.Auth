using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.Services
{
    /// <summary>
    /// Interface representing a tenant data service
    /// </summary>
    public interface ITenantService
    {
        /// <summary>
        /// Create a new tenant
        /// </summary>
        /// <param name="command">The create tenant command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created tenant</returns>
        public Task<Tenant> CreateAsync(CreateTenant command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing tenant
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await</returns>
        public Task DeleteAsync(Guid tenantId, CancellationToken cancellationToken);

        /// <summary>
        /// Ensures that the tenant id exists
        /// </summary>
        /// <param name="id">Unique id of the tenant to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this tenant id exists</returns>
        public Task<bool> ExistsAsync(Guid? id, CancellationToken cancellationToken);

        /// <summary>
        /// Ensures that the tenant name exists
        /// </summary>
        /// <param name="name">Unique name of the tenant to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if this tenant name exists</returns>
        public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a tenant by it's id
        /// </summary>
        /// <param name="tenantId">Unique id of the tenant to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tenant if found or null</returns>
        public Task<Tenant?> GetAsync(Guid tenantId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a tenant by it's name
        /// </summary>
        /// <param name="name">Unique name of the tenant to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Tenant if found or null</returns>
        public Task<Tenant?> GetAsync(string name, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing tenant
        /// </summary>
        /// <param name="command">The update tenant command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated tenant</returns>
        public Task<Tenant> UpdateAsync(UpdateTenant command, CancellationToken cancellationToken);
    }
}
