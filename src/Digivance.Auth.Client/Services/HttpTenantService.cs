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
        public Task<Tenant> CreateAsync(CreateTenant command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetByIdAsync(Guid tenantId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Tenant> UpdateAsync(Guid id, UpdateTenant command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
