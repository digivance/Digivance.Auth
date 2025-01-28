using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// A tenant is a collection of auth related entities such as scopes, user accounts,
    /// roles, permissions, oauth applications and more
    /// </summary>
    public record Tenant : BaseModel
    {
        /// <summary>
        /// User friendly description of this tenant
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User friendly name of this tenant
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The permissions that apply within this tenant (may not be loaded when
        /// tenant is a child)
        /// </summary>
        public ICollection<Permission>? Permissions { get; set; }

        /// <summary>
        /// The roles that exist within this tenant (may not be loaded when tenant
        /// is a child)
        /// </summary>
        public ICollection<Role>? Roles { get; set; }

        /// <summary>
        /// The scopes that exist within this tenant (may not be loaded when tenant is 
        /// a child)
        /// </summary>
        public ICollection<Scope>? Scopes { get; set; }
    }
}
