using Digivance.Core;

namespace Digivance.Auth
{
    /// <summary>
    /// A user identity model is the base representation of a user account
    /// </summary>
    public class UserIdentity : IUserIdentity, IAuditable
    {
        /// <inheritdoc />
        public int CreatedById { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDate { get; set; }

        /// <inheritdoc />
        public string EmailAddress { get; set; }

        /// <inheritdoc />
        public string? FirstName { get; set; }

        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string? LastName { get; set; }

        /// <inheritdoc />
        public int? ModifiedById { get; set; }

        /// <inheritdoc />
        public DateTime? ModifiedDate { get; set; }
    }
}
