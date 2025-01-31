using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Digivance.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        public string Description { get; set; }

        /// <summary>
        /// Unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Collection of the permissions this role assigns
        /// </summary>
        public ICollection<RolePermissionEntity>? RolePermissions { get; set; }

        /// <summary>
        /// The scope that this role exists in
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this role exists in
        /// </summary>
        public Guid ScopeId { get; set; }

        public Role ToModel(int? maxDepth = null, int currentDepth = 0)
        {
            if (maxDepth.HasValue && currentDepth >= maxDepth.Value) return null;

            var role = ToBaseModel<Role>();
            role.Name = Name;
            role.Scope = Scope.ToModel(maxDepth, currentDepth + 1);

            role.Permissions = RolePermissions?.Select(r => r.Permission.ToModel(maxDepth, currentDepth + 1)).ToList();

            return role;
        }
    }

}
