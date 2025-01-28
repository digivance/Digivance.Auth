using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;

namespace Digivance.Auth.Data.EntityFramework.Services
{
    /// <summary>
    /// Entity framework implementation of User service
    /// </summary>
    public class EfUserService : IUserService
    {
        /// <summary>
        /// Internally used AuthContext
        /// </summary>
        private readonly AuthContext dbContext;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="dbContext">The AuthContext to use</param>
        public EfUserService(AuthContext dbContext)
        {
            this.dbContext = dbContext;
        }

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
