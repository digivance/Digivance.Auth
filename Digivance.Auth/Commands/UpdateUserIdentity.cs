namespace Digivance.Auth.Commands
{
    /// <summary>
    /// A command object containing all of the fields necessary to update a user identity
    /// </summary>
    public class UpdateUserIdentity : IUserIdentity
    {
        /// <inheritdoc />
        public string EmailAddress { get; set; }

        /// <inheritdoc />
        public string? FirstName { get; set; }

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string? LastName { get; set; }
    }
}
