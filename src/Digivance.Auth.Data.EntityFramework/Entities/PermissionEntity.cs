using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a permission in our system
    /// </summary>
    public record PermissionEntity : BaseEntity
    {
        /// <summary>
        /// Human friendly description of this permission
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Name of this permission, must be unique per tenant
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigation property of the role permissions containing this permission
        /// </summary>
        public ICollection<RolePermissionEntity> Roles { get; set; }

        /// <summary>
        /// The scope that this permission belongs to
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Unique id of the scope this permission belongs to
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// Navigation property to the tenant that this permission belongs to
        /// </summary>
        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// Unque id of the tenant that this permission applies to
        /// </summary>
        public Guid TenantId { get; set; }

        public ICollection<UserPermissionEntity> Users { get; set; }
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

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Permission);

            builder.HasOne(x => x.Scope)
                .WithMany(x => x.Permissions);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.Permissions);

            // For select by scope id and enforce unique name per scope
            builder.HasIndex(x => new { x.ScopeId, x.Name })
                .IsUnique(true);

            // For select by tenant id
            builder.HasIndex(x => x.TenantId)
                .IsUnique(false);
        }
    }

    /// <summary>
    /// Permission entity mapper configuration
    /// </summary>
    public class PermissionMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for PermissionEntity -> Permission DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PermissionEntity, Permission>()
                .PreserveReferences();
        }
    }
}
