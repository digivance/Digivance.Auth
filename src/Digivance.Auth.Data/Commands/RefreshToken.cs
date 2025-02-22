namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Simple command so clients can send a bearer token refresh code
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// The code to refresh an expired bearer token with
        /// </summary>
        public string Code { get; set; }
    }
}
