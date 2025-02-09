namespace Digivance.Auth.Data.Commands
{
    public record UpdateUser
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string Username { get; set; }
    }
}
