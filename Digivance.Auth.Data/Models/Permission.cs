using Digivance.Data.Models;

namespace Digivance.Auth.Data.Models
{
    /// <summary>
    /// Represents a permission
    /// </summary>
    public record Permission : BaseModel
    {
        /// <summary>
        /// User friendly description of this permission
        /// </summary>
        public string? Description { get; set; }

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
