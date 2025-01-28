using Digivance.Data.EntityFramework.Entities;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// A tenant is a collection of auth related entities such as scopes, user accounts,
    /// roles, permissions, oauth applications and more
    /// </summary>
    public record TenantEntity : BaseEntity
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
        public ICollection<PermissionEntity>? Permissions { get; set; }

        /// <summary>
        /// The roles that exist within this tenant (may not be loaded when tenant
        /// is a child)
        /// </summary>
        public ICollection<RoleEntity>? Roles { get; set; }

        /// <summary>
        /// The scopes that exist within this tenant (may not be loaded when tenant is 
        /// a child)
        /// </summary>
        public ICollection<ScopeEntity>? Scopes { get; set; }
    }
}
