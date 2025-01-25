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
    /// Represents a permission role
    /// </summary>
    public record RoleEntity : BaseEntity
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Collection of the permissions this role assigns
        /// </summary>
        public ICollection<PermissionEntity>? Permissions { get; set; }

        /// <summary>
        /// The scope that this role exists in
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this role exists in
        /// </summary>
        public Guid ScopeId { get; set; }
    }
}
