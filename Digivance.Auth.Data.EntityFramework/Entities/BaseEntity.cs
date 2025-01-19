using Digivance.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents the common fields of all our entities
    /// </summary>
    public record BaseEntity
    {
        /// <summary>
        /// Unique id of the user that created this entity
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// UTC DateTime when this entity was created
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Unique id of this entity
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique id of the user that last modified this entity
        /// </summary>
        public Guid? ModifiedBy { get; set; }

        /// <summary>
        /// UTC DateTime when this model was last modified
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Helper that constructs the requested model type with this entities base fields
        /// </summary>
        /// <typeparam name="T">The type of model to create</typeparam>
        /// <returns>new T with base fields set from this entity</returns>
        public T ToBaseModel<T>()
            where T : BaseModel, new()
        {
            return new T
            {
                CreatedBy = CreatedBy,
                CreatedOn = CreatedOn,
                Id = Id,
                ModifiedBy = ModifiedBy,
                ModifiedOn = ModifiedOn
            };
        }
    }

    /// <summary>
    /// Helper extensions related to our base model
    /// </summary>
    public static class BaseEntityConfiguration
    {
        /// <summary>
        /// Allows models that inherit from BaseModel to configure their BaseModel fields...
        /// </summary>
        /// <typeparam name="T">The type of entity model we are configuring (must derrive from BaseModel)</typeparam>
        /// <param name="builder">The entity type builder to configure</param>
        public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder)
            where T : BaseEntity
        {
            builder.Property(x => x.CreatedBy)
                .IsRequired(false);

            builder.Property(x => x.CreatedOn)
                .IsRequired(true);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ModifiedBy)
                .IsRequired(false);

            builder.Property(x => x.ModifiedOn)
                .IsRequired(false);
        }
    }
}
