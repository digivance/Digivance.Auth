using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents a permission
    /// </summary>
    public record Permission : BaseModel
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// User friendly name of this permission
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Optional list of roles that contain this permission
        /// </summary>
        public ICollection<Role>? Roles { get; set; }


        /// <summary>
        /// The scope that this permission exists in
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this permission belongs to
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// The tenant that this permission belongs to (may not be loaded when scope
        /// is a child, always trust TenantId instead)
        /// </summary>
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Unique id of the Tenant this permission belongs to
        /// </summary>
        public Guid TenantId { get; set; }
    }
}
