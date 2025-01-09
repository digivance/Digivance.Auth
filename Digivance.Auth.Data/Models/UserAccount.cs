using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents a user account / identity
    /// </summary>
    public record UserAccount : BaseModel
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Required unique email address of this user account
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// UTC Date and Time when this user verified this email address
        /// </summary>
        public DateTime? EmailVerifiedOn { get; set; }

        /// <summary>
        /// Identifies if this user account has verified their email address
        /// </summary>
        public bool IsEmailVerified { get; set; } = false;

        /// <summary>
        /// Permissions this user has (including those granted via roles)
        /// </summary>
        public ICollection<Permission> Permissions { get; set; }

        /// <summary>
        /// Roles this user is assigned to
        /// </summary>
        public ICollection<Role> Roles { get; set; }

        /// <summary>
        /// The tenant that this user account exists in (may not be set when useraccount is
        /// a child entity)
        /// </summary>
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Unique id of the tenant that this user account exists in
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string? Username { get; set; }
    }
}
