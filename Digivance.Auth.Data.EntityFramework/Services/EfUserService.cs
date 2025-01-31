using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.EntityFramework.Contexts;
using Digivance.Auth.Data.EntityFramework.Entities;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

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

        public async Task<UserAccount> CreateAsync(CreateUser command, CancellationToken cancellationToken)
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

        public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EmailAddressExistsAsync(string emailAddress, CancellationToken cancellationToken)
        {
            var isTaken = await db.UserAccounts
                .Where(x => x.EmailAddress == emailAddress)
                .AnyAsync();

            return (isTaken);
        }

        public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
        {
            var isTaken = await db.UserAccounts
                .Where(x => x.Username == username)
                .AnyAsync();

            return (isTaken);
        }

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<UserAccount?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
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

        public async Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await db.UserAccounts
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.ToModel(maxDepth: 1);
        }

        public async Task<UserAccount?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await db.UserAccounts
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            return user.ToModel(maxDepth:1);
        }

        public Task<UserAccount> UpdateAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
