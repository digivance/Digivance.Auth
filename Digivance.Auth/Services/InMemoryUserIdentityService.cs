using Digivance.Auth.Commands;

namespace Digivance.Auth.Services
{
    /// <summary>
    /// Very basic implementation of UserIdentityService in memory, note that the Users
    /// collecion is static to support scoped dependancy injection.
    /// </summary>
    public class InMemoryUserIdentityService : IUserIdentityService
    {
        /// <summary>
        /// The "database" of user identities
        /// </summary>
        protected static List<UserIdentity> Users = new List<UserIdentity>();

        /// <inheritdoc />
        public Task<UserIdentity> CreateUserIdentityAsync(CreateUserIdentity command, CancellationToken cancellationToken = default)
        {
            if (Users.Any(user => user.EmailAddress == command.EmailAddress))
                throw new Exception("Email address already in use");

            var newUserIdentity = new UserIdentity
            {
                CreatedById = 0,
                CreatedDate = DateTime.UtcNow,
                EmailAddress = command.EmailAddress,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            Users.Add(newUserIdentity);
            return Task.FromResult(newUserIdentity);
        }

        /// <inheritdoc />
        public Task DeleteUserIdentityAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingUserIdentity = Users.FirstOrDefault(user => user.Id == id);
            if (existingUserIdentity != null)
                Users.Remove(existingUserIdentity);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task<UserIdentity> GetUserIdentityAsync(int id, CancellationToken cancellationToken = default)
        {
            var existingUser = Users.FirstOrDefault(user => user.Id == id);
            if (existingUser == null)
                throw new Exception("User not found");

            return Task.FromResult(existingUser);
        }

        /// <inheritdoc />
        public Task<UserIdentity> UpdateAsync(UpdateUserIdentity command, CancellationToken cancellationToken = default)
        {
            var existingUser = Users.FirstOrDefault(user => user.Id == command.Id);
            if (existingUser == null)
                throw new Exception("User not found");

            existingUser.EmailAddress = command.EmailAddress;
            existingUser.FirstName = command.FirstName;
            existingUser.LastName = command.LastName;
            existingUser.ModifiedDate = DateTime.UtcNow;
            existingUser.ModifiedById = 0;

            return Task.FromResult(existingUser);
        }
    }
}
