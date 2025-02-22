using Digivance.Auth.Data.Models;
using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AutoMapper;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a user account / identity
    /// </summary>
    public record UserEntity : BaseEntity
    {
        /// <summary>
        /// Optional display name that the user can choose to identify themselves as
        /// </summary>
        public string? DisplayName { get; set; }

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

        /// <summary>
        /// Hashed password, never expose this
        /// </summary>
        [JsonIgnore]
        public byte[] Password { get; set; }

        /// <summary>
        /// Relations explaining permissions explicitly granted to this user
        /// </summary>
        public ICollection<UserPermissionEntity> Permissions { get; set; }

        /// <summary>
        /// Internally used (not returned in models) these are the currently valid
        /// codes that can be used to refresh bearer tokens
        /// </summary>
        public ICollection<UserRefreshCodeEntity> RefreshCodes { get; set; }

        /// <summary>
        /// Relations explaining roles this user is assigned
        /// </summary>
        public ICollection<UserRoleEntity> Roles { get; set; }

        /// <summary>
        /// The tenant that this user account exists in
        /// </summary>
        public TenantEntity Tenant { get; set; }

        /// <summary>
        /// Unique id of the tenant that this user account exists in
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Optional, unique per tenant if provided, custom username of this user account
        /// </summary>
        public string? Username { get; set; }
    }

    /// <summary>
    /// Entity type configuration for our UserProfileEntity
    /// </summary>
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        /// <summary>
        /// Configures ef for our UserEntity
        /// </summary>
        /// <param name="builder">The builder to configure</param>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(x => x.EmailAddress)
                .HasMaxLength(256)
                .IsRequired(true);

            builder.HasIndex(x => x.EmailAddress)
                .IsUnique(true);

            builder.Property(x => x.IsEmailVerified)
                .HasDefaultValue(false) // It does have a value... of false
                .IsRequired(true);

            builder.Property(x => x.Password)
                .HasMaxLength(512);

            builder.HasMany(x => x.Permissions)
                .WithOne(x => x.User);

            builder.HasMany(x => x.RefreshCodes)
                .WithOne(x => x.User);

            builder.HasMany(x => x.Roles)
                .WithOne(x => x.User);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.UserAccounts);

            builder.Property(x => x.Username)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.HasIndex(x => new { x.TenantId, x.Username })
                .IsUnique(true);
        }
    }

    /// <summary>
    /// User entity mapper configuration
    /// </summary>
    public class UserMapperConfiguration : IEntityMapperConfiguration
    {
        /// <summary>
        /// Configures automapper for UserEntity -> User DTO
        /// </summary>
        /// <param name="cfg">The configuration builder to use</param>
        public void Configure(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserEntity, User>()
                .PreserveReferences();
        }
    }

}
