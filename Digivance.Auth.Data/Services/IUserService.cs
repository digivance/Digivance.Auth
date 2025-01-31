using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.Services
{
    /// <summary>
    /// Interface representing a user data service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="command">The create user command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created user profile</returns>
        public Task<UserAccount> CreateAsync(CreateUser command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="userId">Unique id of the user to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await</returns>
        public Task DeleteByIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if this email address is already in use
        /// </summary>
        /// <param name="emailAddress">The email address to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email address is taken</returns>
        public Task<bool> EmailAddressExists(string emailAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if this is a valid user id
        /// </summary>
        /// <param name="id">Unique id of the user to check for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if there is a user by this id</returns>
        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);


        /// <summary>
        /// Returns a user account by email address
        /// </summary>
        /// <param name="emailAddress">Email address of the user to get</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<UserAccount?> GetByEmailAddress(string emailAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a user account by it's username
        /// </summary>
        /// <param name="username">Unique username of the user to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UserProfile if found or null</returns>
        public Task<UserAccount?> GetByUsernameAsync(string username, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a user account by it's id
        /// </summary>
        /// <param name="userId">Unique id of the user to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UserProfile if found or null</returns>
        public Task<UserAccount?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="command">The update user command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated UserProfile</returns>
        public Task<UserAccount> UpdateAsync(Guid id, UpdateUser command, CancellationToken cancellationToken);
    }
}
