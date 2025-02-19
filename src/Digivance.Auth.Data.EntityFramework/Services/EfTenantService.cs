using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;

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
        private readonly AuthContext dbContext;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="dbContext">The AuthContext to use</param>
        public EfTenantService(AuthContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<Tenant> CreateTenantAsync(CreateTenant command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetTenantByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetTenantByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TenantExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TenantNameExistsAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant> UpdateTenantAsync(Guid id, UpdateTenant command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
