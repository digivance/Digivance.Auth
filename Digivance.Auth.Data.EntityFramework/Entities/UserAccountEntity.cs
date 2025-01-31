using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Digivance.Data.Models;
using System.Reflection.Metadata;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a user account / identity
    /// </summary>
    public record UserAccountEntity : BaseEntity
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Required unique email address of this user account
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// UTC Date and Time when this user verified this email address
        /// </summary>
        public DateTime? EmailVerifiedOn { get; set; }

        /// <summary>
        /// Identifies if this user account has verified their email address
        /// </summary>
        public bool IsEmailVerified { get; set; } = false;

        public string Password { get; set; }

        /// <summary>
        /// Roles this user is assigned to
        /// </summary>
        public ICollection<RolePermissionEntity>? RolePermissions { get; set; }

        /// <summary>
        /// The tenant that this user account exists in (may not be set when useraccount is
        /// a child entity)
        /// </summary>
        public TenantEntity? Tenant { get; set; }

        /// <summary>
        /// Unique id of the tenant that this user account exists in
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string Username { get; set; }

        public UserAccount ToModel(int? maxDepth = 0, int currentDepth = 0)
        {
            if (maxDepth.HasValue && currentDepth >= maxDepth.Value) return null;

            var user = ToBaseModel<UserAccount>();
            user.DisplayName = DisplayName;
            user.EmailAddress = EmailAddress;
            user.EmailVerifiedOn = EmailVerifiedOn;
            user.IsEmailVerified = IsEmailVerified;
            user.Tenant = Tenant?.ToModel(maxDepth, currentDepth +1);
            user.TenantId = TenantId;
            user.Username = Username;

            return user;
        }
    }

    /// <summary>
    /// Entity type configuration for our UserProfileEntity
    /// </summary>
    public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserAccountEntity>
    {
        /// <summary>
        /// Configures ef for our UserProfileEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.EmailAddress)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.HasIndex(x => x.EmailAddress)
                .IsUnique(true);

            builder.Property(x => x.DisplayName)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(x => x.IsEmailVerified)
                .HasDefaultValue(false)
                .IsRequired(true);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.UserAccounts);
        }
    }


}
