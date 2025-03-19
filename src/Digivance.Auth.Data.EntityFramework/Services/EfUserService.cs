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
    /// <param name="context">The AuthContext to use</param>
    /// <param name="mapper">The EntityMapper to use when converting entities to DTO models</param>
    public class EfUserService(AuthContext context, EntityMapper mapper) : IUserService
    {
        /// <summary>
        /// Internally used AuthContext
        /// </summary>
        private readonly AuthContext context = context;

        /// <summary>
        /// Internally used EntityMapper
        /// </summary>
        private readonly EntityMapper mapper = mapper;

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
        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
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
        public Task<bool> ExistsByEmailAsync(Guid? tenantId, string emailAddress, CancellationToken cancellationToken)
        {
            if (tenantId == new Guid())
                tenantId = null;

            return context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.EmailAddress == emailAddress)
                .AnyAsync(cancellationToken);
        }

        /// <inheritdoc />
        public Task<bool> ExistsByUsernameAsync(Guid? tenantId, string username, CancellationToken cancellationToken)
        {
            if (tenantId == new Guid())
                tenantId = null;

            return context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username == username)
                .AnyAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<User?> GetByEmailAddressAsync(Guid? tenantId, string emailAddress, CancellationToken cancellationToken)
        {
            if (tenantId == new Guid())
                tenantId = null;

            var user = await context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.EmailAddress == emailAddress)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Include(x => x.Permissions)
                    .ThenInclude(x => x.Permission)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> GetByUsernameAsync(Guid? tenantId, string username, CancellationToken cancellationToken)
        {
            if (tenantId == new Guid())
                tenantId = null;

            var user = await context.UserAccounts
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            return mapper.Map<User>(user);
        }

        /// <inheritdoc />
        public async Task<User?> UpdateAsync(UpdateUser command, CancellationToken cancellationToken)
        {
            var user = await context.UserAccounts
                .Where(x => x.Id == command.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null;

            user.DisplayName = command.DisplayName;
            user.Username = command.Username;

            await context.SaveChangesAsync(cancellationToken);
            return mapper.Map<User>(user);
        }
    }
}
