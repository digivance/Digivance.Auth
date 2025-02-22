using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;
using Digivance.Auth.Data.Services;

namespace Digivance.Auth.Data.Tests.Services
{
    /// <summary>
    /// In memory implementation of our IUserService to use for unit testing our validators
    /// </summary>
    public class InMemoryUserService : IUserService
    {
        private List<User> users = new List<User>();

        /// <inheritdoc />
        public Task<User> CreateAsync(CreateUser command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                CreatedOn = DateTime.UtcNow,
                DisplayName = command.DisplayName,
                EmailAddress = command.EmailAddress,
                Id = Guid.NewGuid(),
                TenantId = command.TenantId,
                Username = command.Username
            };

            users.Add(user);
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
                users.Remove(user);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            var exists = users.Any(x => x.Id == id);
            return Task.FromResult(exists);
        }

        /// <inheritdoc />
        public Task<bool> ExistsByEmailAsync(Guid? tenantId, string emailAddress, CancellationToken cancellationToken)
        {
            var exists = users.Any(x => x.TenantId == tenantId && x.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }

        /// <inheritdoc />
        public Task<bool> ExistsByUsernameAsync(Guid? tenantId, string? username, CancellationToken cancellationToken)
        {
            if (username == null)
                return Task.FromResult(false);

            var exists = users
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                .Any();

            return Task.FromResult(exists);
        }

        /// <inheritdoc />
        public Task<User?> GetByEmailAddressAsync(Guid? tenantId, string emailAddress, CancellationToken cancellationToken)
        {
            var user = users
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.EmailAddress.Equals(emailAddress, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = users.FirstOrDefault(x => x.Id == userId);
            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<User?> GetByUsernameAsync(Guid? tenantId, string username, CancellationToken cancellationToken)
        {
            var user = users
                .Where(x => x.TenantId == tenantId)
                .Where(x => x.Username != null && x.Username.Equals(username, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            return Task.FromResult(user);
        }

        /// <inheritdoc />
        public Task<User?> UpdateAsync(UpdateUser command, CancellationToken cancellationToken)
        {
            var user = users.FirstOrDefault(x => x.Id == command.Id);
            if (user != null)
            {
                user.DisplayName = command.DisplayName;
                user.Username = command.Username;
            }

            return Task.FromResult(user);
        }
    }
}
