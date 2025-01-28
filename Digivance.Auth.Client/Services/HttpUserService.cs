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
        public Task<UserAccount> CreateUserAsync(CreateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailAddressTaken(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetUserByEmailAddress(string emailAddress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetUserByHandleAsync(string handle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserAccount> UpdateUserAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
