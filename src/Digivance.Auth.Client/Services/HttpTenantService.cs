using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;

namespace Digivance.Auth.Client.Services
{
    /// <summary>
    /// This implementation will make API calls to the Digivance.Auth.Api service to interact
    /// with tenant records
    /// </summary>
    public class HttpTenantService : ITenantService
    {
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
