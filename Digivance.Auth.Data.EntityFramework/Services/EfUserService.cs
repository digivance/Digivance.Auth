using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;

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
        private readonly AuthContext db;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="dbContext">The AuthContext to use</param>
        public EfUserService(AuthContext dbContext)
        {
            this.db = dbContext;
        }

        public async Task<UserAccount> CreateUserAsync(CreateUser command, CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid();

            var user = new UserAccountEntity
            {
                Id = userId,
                EmailAddress = command.EmailAddress,
                Password = command.Password,
                TenantId = command.TenantId,
                DisplayName = command.DisplayName,
                Username = command.Username
            };

            db.UserAccounts.Add(user);
            await db.SaveChangesAsync(cancellationToken);
            return user.ToModel(maxDepth:1);
        }

        public Task DeleteUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailAddressTaken(string emailAddress, CancellationToken cancellationToken)
        {
            var user = await db.UserAccounts
                .Where(x => x.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            return (user != null);
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserAccount?> GetUserByEmailAddress(string emailAddress, CancellationToken cancellationToken)
        {
            var user = await db.UserAccounts
                .Where(x => x.EmailAddress == emailAddress)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.ToModel(maxDepth:1);
        }

        public Task<UserAccount?> GetUserByHandleAsync(string handle, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserAccount?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await db.UserAccounts
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.ToModel(maxDepth:1);
        }

        public Task<UserAccount> UpdateUserAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
