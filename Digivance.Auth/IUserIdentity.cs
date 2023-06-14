using Digivance.Core;

namespace Digivance.Auth
{
    /// <summary>
    /// Used to indicate that an object has basic user identity, and auditing fields
    /// </summary>
    public interface IUserIdentity
    {
        /// <summary>
        /// The user's email address
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string? LastName { get; set; }
    }
}
