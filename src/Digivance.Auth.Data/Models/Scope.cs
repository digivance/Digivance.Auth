using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents a scope within a tenant. A scope represents a collection of content
    /// and allows our users to create permissions that apply to specific sub collections.
    /// For example, considering a social media site, a scope might represent a group,
    /// and permissions that apply to all entities in this groups scope can administrate
    /// all posts in that group.
    /// </summary>
    public record Scope : BaseModel
    {
        /// <summary>
        /// User friendly description of this scope
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User friendly name of this scope
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The permissions that apply to this scope (may not be loaded when scope
        /// is a child)
        /// </summary>
        public ICollection<Permission>? Permissions { get; set; }

        /// <summary>
        /// The roles that apply to this scope (may not be loaded when scope is a child)
        /// </summary>
        public ICollection<Role>? Roles { get; set; }

        /// <summary>
        /// Unique id of the Tenant this scope belongs to
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// The tenant that this scope belongs to (may not be loaded when scope
        /// is a child, always trust TenantId instead)
        /// </summary>
        public Tenant? Tenant { get; set; }
    }
}
