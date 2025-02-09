using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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

        /// <summary>
        /// The user accounts that exist within this tenant (may not be loaded when tenant
        /// is a child)
        /// </summary>
        public ICollection<UserEntity>? UserAccounts { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for our TenantEntity
    /// </summary>
    public class TenantEntityConfiguration : IEntityTypeConfiguration<TenantEntity>
    {
        /// <summary>
        /// Configures EF for our TenantEntity
        /// </summary>
        /// <param name="builder">Builder to configure</param>
        public void Configure(EntityTypeBuilder<TenantEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Description)
                .HasMaxLength(4000)
                .IsRequired(false);

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.Tenant);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.Tenant);

            builder.HasMany(x => x.Scopes)
                .WithOne(x => x.Tenant);

            builder.HasMany(x => x.UserAccounts)
                .WithOne(x => x.Tenant);
        }
    }

    /// <summary>
    /// Tenant entity mapper configuration
    /// </summary>
    public class TenantMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for TenantEntity -> Tenant DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TenantEntity, Tenant>()
                .PreserveReferences();
        }
    }
}
