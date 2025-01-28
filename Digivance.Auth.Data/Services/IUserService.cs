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
        public Task<UserAccount> CreateUserAsync(CreateUser command, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="userId">Unique id of the user to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task to await</returns>
        public Task DeleteUserByIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Checks to see if this email address is already in use
        /// </summary>
        /// <param name="emailAddress">The email address to look for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email address is taken</returns>
        public Task<bool> EmailAddressTaken(string emailAddress, CancellationToken cancellationToken);

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
        public Task<UserAccount?> GetUserByEmailAddress(string emailAddress, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a user account by it's handle
        /// </summary>
        /// <param name="handle">Unique handle of the user to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UserProfile if found or null</returns>
        public Task<UserAccount?> GetUserByHandleAsync(string handle, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a user account by it's id
        /// </summary>
        /// <param name="userId">Unique id of the user to get</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>UserProfile if found or null</returns>
        public Task<UserAccount?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="command">The update user command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated UserProfile</returns>
        public Task<UserAccount> UpdateUserAsync(Guid id, UpdateUser command, CancellationToken cancellationToken);
    }
}
