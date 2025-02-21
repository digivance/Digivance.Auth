namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Model representing a successful authentication result
    /// </summary>
    public record AuthenticationResult
    {
        /// <summary>
        /// UTC Date and time when this bearer token will expire
        /// </summary>
        public DateTime BearerExpires { get; set; }

        /// <summary>
        /// The JWT Bearer token for the authenticated user
        /// </summary>
        public string BearerToken { get; set; }

        /// <summary>
        /// Refresh code for getting a new bearer token
        /// </summary>
        public string RefreshCode { get; set; }

        /// <summary>
        /// UTC Date and time when this refresh token will expire
        /// </summary>
        public DateTime RefreshExpires { get; set; }
    }
}
