namespace Digivance.Auth.Commands
{
    /// <summary>
    /// A command object containing all of the necessary fields to create a new
    /// user identity.
    /// </summary>
    public class CreateUserIdentity : IUserIdentity
    {
        /// <inheritdoc />
        public string EmailAddress { get; set; }

        /// <inheritdoc />
        public string? FirstName { get; set; }

        /// <inheritdoc />
        public string? LastName { get; set; }

        /// <summary>
        /// Caution this is expected to be a clear text password, always make sure your
        /// site is https when the user sends this over the wire.
        /// </summary>
        public string Password { get; set; }
    }
}
