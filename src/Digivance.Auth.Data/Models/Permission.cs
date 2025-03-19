using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents our common system level permissions such as basic CRUD operations
    /// </summary>
    public static class EntityAccess
    {
        /// <summary>
        /// Can create this type of entity, only applies when EntityId is null
        /// </summary>
        public const string Create = "Create";

        /// <summary>
        /// Can delete this type of entity
        /// </summary>
        public const string Delete = "Delete";

        /// <summary>
        /// Can read this type of entity
        /// </summary>
        public const string Read = "Read";

        /// <summary>
        /// Can update this type of entity
        /// </summary>
        public const string Update = "Update";
    }

    /// <summary>
    /// Represents a permission in the system. These permissions are loosely entity based,
    /// meaning that it is expected for these permissions to apply to a specific entity,
    /// or entity type.
    /// </summary>
    public record Permission : BaseModel
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// This is a string representing the access that this permission allows. Can
        /// be one of the EntityAccess system permissions or custom
        /// </summary>
        public string EntityAccess { get; set; }

        /// <summary>
        /// This is the unique id of the entity that this permission is granting access to.
        /// If null, it is granting this EntityAccess to ALL entities of this type. If null,
        /// this permission may be limited to ScopeId.  See ScopeId
        /// </summary>
        public Guid? EntityId { get; set; }

        /// <summary>
        /// The type name of the entity that this permission applies to (such as the name
        /// of the model that this permission applies to)
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Optional list of roles that contain this permission
        /// </summary>
        public ICollection<Role>? Roles { get; set; }

        /// <summary>
        /// The scope that this permission grants access to. Always present but only applicable 
        /// when EntityId is null, See Scope
        /// </summary>
        public Scope Scope { get; set; }

        /// <summary>
        /// Unique id of the scope that this permission belongs to. Always present but only 
        /// applicable when EntityId is null, See Scope
        /// </summary>
        public Guid ScopeId { get; set; }
    }
}
