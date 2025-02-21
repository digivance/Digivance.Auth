using Digivance.Data.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// UserRefreshCode contains a security code that can be used to refresh a user bearer token
    /// </summary>
    public record UserRefreshCodeEntity : BaseEntity
    {
        /// <summary>
        /// The security code that can be used to refresh an expired bearer token
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// UTC DateTime when this refresh code expires and should be removed
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// The user this refresh code is good for
        /// </summary>
        public UserEntity User { get; set; }

        /// <summary>
        /// Unique id of the user this refresh code is good for
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// Entity framework configuration for our UserRefreshCode entity
    /// </summary>
    public class UserRefreshCodeConfiguration : IEntityTypeConfiguration<UserRefreshCodeEntity>
    {
        /// <summary>
        /// Configures entity framework for our UserRefreshToken entity
        /// </summary>
        /// <param name="builder">The entity type builder to configure</param>
        public void Configure(EntityTypeBuilder<UserRefreshCodeEntity> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(x => x.Code)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.UserId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.RefreshCodes);
        }
    }
}
