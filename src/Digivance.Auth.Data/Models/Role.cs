using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents a permission role
    /// </summary>
    public record Role : BaseModel
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Collection of the permissions this role assigns
        /// </summary>
        public ICollection<Permission>? Permissions { get; set; }

        /// <summary>
        /// The scope that this role exists in
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this role exists in
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// The tenant this role belongs to
        /// </summary>
        public Tenant Tenant { get; set; }

        /// <summary>
        /// Unique id of the tenant this role belongs to
        /// </summary>
        public Guid TenantId { get; set; }
    }
}
