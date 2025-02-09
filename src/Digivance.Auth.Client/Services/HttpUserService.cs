using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;

namespace Digivance.Auth.Client.Services
{
    /// <summary>
    /// This implementation will make API calls to the Digivance.Auth.Api service to interact
    /// with user records
    /// </summary>
    public class HttpUserService : IUserService
    {
        public Task<User> CreateAsync(CreateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByEmailAsync(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsByUsernameAsync(Guid tenantId, string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByUsernameAsync(Guid tenantId, string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> UpdateAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
