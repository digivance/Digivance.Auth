using Digivance.Data.EntityFramework.Entities;
using Digivance.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a scope within a tenant. Scopes can be anything the user needs.
    /// </summary>
    public record ScopeEntity : BaseEntity
    {
        /// <summary>
        /// User friendly description of this scope
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// User friendly name of this scope
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The permissions that apply to this scope (may not be loaded when scope
        /// is a child)
        /// </summary>
        public ICollection<PermissionEntity>? Permissions { get; set; }

        /// <summary>
        /// The roles that apply to this scope (may not be loaded when scope is a child)
        /// </summary>
        public ICollection<RoleEntity>? Roles { get; set; }

        /// <summary>
        /// Unique id of the Tenant this scope belongs to
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// The tenant that this scope belongs to (may not be loaded when scope
        /// is a child, always trust TenantId instead)
        /// </summary>
        public TenantEntity? Tenant { get; set; }
    }
}
