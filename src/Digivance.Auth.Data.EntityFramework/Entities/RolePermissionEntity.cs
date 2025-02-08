using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Relation entity representing what permissions are assigned to what roles
    /// </summary>
    public record RolePermissionEntity : BaseEntity
    {
        /// <summary>
        /// Navigation property to the permission this relation applies to
        /// </summary>
        public PermissionEntity Permission { get; set; }

        /// <summary>
        /// Unique id of the permission this relation applies to
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// Navigation property of the role that this relation applies to
        /// </summary>
        public RoleEntity Role { get; set; }

        /// <summary>
        /// Unique id of the role that this relation applies to
        /// </summary>
        public Guid RoleId { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for our RolePermissionEntity
    /// </summary>
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        /// <summary>
        /// Configures EF for our RolePermissionEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.PermissionId);
            builder.HasIndex(x => x.RoleId);
        }
    }
}
