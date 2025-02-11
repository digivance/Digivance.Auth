namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to update a permission record
    /// </summary>
    public record UpdatePermission
    {
        /// <summary>
        /// Human friendly description of this permission
        /// </summary>
        public string Description { get; set; }
    }
}
