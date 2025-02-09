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
        private readonly AuthContext context;

        /// <summary>
        /// Internally used EntityMapper
        /// </summary>
        private readonly EntityMapper mapper;

        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="context">The AuthContext to use</param>
        /// <param name="mapper">The EntityMapper we will use when converting to DTOs</param>
        public EfUserService(AuthContext context, EntityMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<User> CreateAsync(CreateUser command, CancellationToken cancellationToken)
        {
            var user = new UserEntity
            {
                EmailAddress = command.EmailAddress,
                Password = PasswordHelper.Hash(command.Password),
                TenantId = command.TenantId,
                DisplayName = command.DisplayName,
                Username = command.Username
            };

            context.UserAccounts.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                context.UserAccounts.Remove(user);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
            => context.UserAccounts
                .Where(x => x.Id == id)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsByEmailAsync(string emailAddress, CancellationToken cancellationToken)
            => context.UserAccounts
                .Where(x => x.EmailAddress == emailAddress)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public Task<bool> ExistsByUsernameAsync(Guid tenantId, string username, CancellationToken cancellationToken)
            => context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username == username)
                .AnyAsync(cancellationToken);

        /// <inheritdoc />
        public async Task<User?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Where(x => x.EmailAddress == emailAddress)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByUsernameAsync(Guid tenantId, string username, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> UpdateAsync(Guid id, UpdateUser command, CancellationToken cancellationToken)
        {
            var user = await GetByIdAsync(id, cancellationToken);

            if (user == null)
                return null;

            user.DisplayName = command.DisplayName;
            user.Username = command.Username;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<User>(user);
        }
    }
}
