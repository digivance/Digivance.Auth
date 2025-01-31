using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

        public ICollection<UserAccountEntity>? UserAccounts { get; set; }

        public Tenant ToModel(int? maxDepth = null, int currentDepth = 0)
        {
            if (maxDepth.HasValue && currentDepth >= maxDepth.Value) return null;

            var tenant = ToBaseModel<Tenant>();
            tenant.Name = Name;
            tenant.Permissions = Permissions?.Select(x => x.ToModel(maxDepth, currentDepth + 1)).ToList();
            tenant.Roles = Roles?.Select(x => x.ToModel(maxDepth, currentDepth + 1)).ToList();
            tenant.Scopes = Scopes?.Select(x => x.ToModel(maxDepth, currentDepth + 1)).ToList();
            tenant.UserAccounts = UserAccounts?.Select(x => x.ToModel(maxDepth, currentDepth + 1)).ToList();
            return tenant;
        }
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

            builder.Property(x => x.Name)
                .HasMaxLength(255)
                .IsRequired(true);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            builder.HasMany(x => x.Scopes)
                .WithOne(x => x.Tenant);
        }
    }
}
