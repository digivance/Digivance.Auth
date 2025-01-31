using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Digivance.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a permission in our system
    /// </summary>
    public record PermissionEntity : BaseEntity
    {
        /// <summary>
        /// Name of this permission, must be unique per tenant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigation property of the role permissions containing this permission
        /// </summary>
        public ICollection<RolePermissionEntity> RolePermissions { get; set; }

        /// <summary>
        /// Navigation property to the tenant that this permission applies to
        /// </summary>
        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// Unque id of the tenant that this permission applies to
        /// </summary>
        public Guid TenantId { get; set; }


        /// <summary>
        /// The scope that this permission exists in
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Navigation property to the user permissions containing this permission
        /// </summary>
        /*public ICollection<UserPermissionEntity> UserPermissions { get; set; }*/


        public Permission ToModel(int? maxDepth = null, int currentDepth = 0)
        {
            if (maxDepth != null && currentDepth >= maxDepth.Value) return null;

            var perm = ToBaseModel<Permission>();
            perm.Name = Name;
            perm.Roles = RolePermissions?.Select(r => r.Role.ToModel(maxDepth, currentDepth + 1)).ToList();
            perm.Scope = Scope.ToModel(maxDepth, currentDepth + 1);
            perm.Tenant = Tenant?.ToModel(maxDepth, currentDepth + 1);

            return perm;
        }
    }

    /// <summary>
    /// Entity framework configuration for PermissionEntity
    /// </summary>
    public class PermissionEntityConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        /// <summary>
        /// Configures EF for our PermissionEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true)
                .IsUnicode(false);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.Permissions);

            builder.HasMany(x => x.RolePermissions)
                .WithOne(x => x.Permission);

            builder.HasIndex(x => new { x.Name, x.TenantId })
                .IsUnique(true);
        }
    }
}
