namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to authenticate a user account via user credentials
    /// </summary>
    public record AuthenticateUserCredentials
    {
        /// <summary>
        /// Email address of the account to authenticate with
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Clear text password to authenticate with
        /// </summary>
        public string Password { get; set; }
    }
}
