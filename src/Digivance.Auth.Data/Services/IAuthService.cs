using Digivance.Auth.Data.Commands;
using Digivance.Auth.Data.Models;

namespace Digivance.Auth.Data.Services
{
    /// <summary>
    /// Interface representing our authentication service
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate a user account using traditional credentials
        /// </summary>
        /// <param name="command">The AuthenticateUserCredentials command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>AuthenticationResult if successful, or throws FluentValidation.ValidationException</returns>
        public Task<AuthenticationResult> AuthenticateAsync(AuthenticateUserCredentials command, CancellationToken cancellationToken);

        /// <summary>
        /// Refresh a user account using a valid but expired jwt with refresh code
        /// </summary>
        /// <param name="expiredJwt">The current expired bearer token token</param>
        /// <param name="refreshCode">A valid refresh code received from authenticate</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>AuthenticationResult if successful, or throws FluentValidation.ValidationException</returns>
        public Task<AuthenticationResult> RefreshTokenAsync(string expiredJwt, string refreshCode, CancellationToken cancellationToken);
    }
}
