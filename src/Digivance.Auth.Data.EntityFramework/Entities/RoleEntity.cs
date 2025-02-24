using AutoMapper;
using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        public ICollection<RolePermissionEntity>? Permissions { get; set; }

        /// <summary>
        /// The scope that this role belongs to
        /// </summary>
        public ScopeEntity Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this role belongs to
        /// </summary>
        public Guid ScopeId { get; set; }

        /// <summary>
        /// The tenant that this role belongs to
        /// </summary>
        public TenantEntity? Tenant { get; set; }

        /// <summary>
        /// Unique id of the tenant that this role belongs to
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Users that are assigned this role
        /// </summary>
        public ICollection<UserRoleEntity>? Users { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for RoleEntity
    /// </summary>
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        /// <summary>
        /// Configures EF for our RoleEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Role);

            builder.HasOne(x => x.Scope)
                .WithMany(x => x.Roles);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.Roles);

            // For select by scope id and enforce unique per scope
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
    public class RoleMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for RoleEntity -> Role DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<RoleEntity, Role>()
                .PreserveReferences();
        }
    }
}
