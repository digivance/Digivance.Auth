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
        public Task<UserAccount> CreateAsync(CreateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailAddressExistsAsync(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount> UpdateAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
