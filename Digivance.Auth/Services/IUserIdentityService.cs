using Digivance.Auth.Commands;

namespace Digivance.Auth.Services
{
    /// <summary>
    /// Used to indicate a service object capable of handling User Identity functionality
    /// </summary>
    public interface IUserIdentityService
    {
        /// <summary>
        /// Create and persist the new user identity record, returns a model representing 
        /// the newly created user identity
        /// </summary>
        /// <param name="command">The CreateUserIdentity command to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>UserIdentity model representing the newly created UserIdentity</returns>
        public Task<UserIdentity> CreateUserIdentityAsync(CreateUserIdentity command, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a UserIdentity from storage
        /// </summary>
        /// <param name="id">Unique id of the UserIdentity to remove</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task to await</returns>
        public Task DeleteUserIdentityAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a UserIdentity by unique id
        /// </summary>
        /// <param name="id">Unique id of the UserIdentity to return</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>UserIdentity model representing the requested user UserIdentity</returns>
        public Task<UserIdentity> GetUserIdentityAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates and persists a UserIdentity and returns a UserIdentity model representing 
        /// the updated user identity
        /// </summary>
        /// <param name="command">The UpdateUserIdentity command to execute</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>UserIdentity model representing the updated UserIdentity</returns>
        public Task<UserIdentity> UpdateAsync(UpdateUserIdentity command, CancellationToken cancellationToken = default);
    }
}
