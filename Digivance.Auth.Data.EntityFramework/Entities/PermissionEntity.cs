using Digivance.Data.EntityFramework.Entities;

namespace Digivance.Auth.Data.EntityFramework.Entities
{
    /// <summary>
    /// Represents a permission in our system
    /// </summary>
    public record PermissionEntity : BaseEntity
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User friendly name of this permission
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Unique id of the scope that this permission belongs to
        /// </summary>
        public Guid ScopeId { get; set; }
    }
}
