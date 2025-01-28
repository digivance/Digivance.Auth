namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new permission
    /// </summary>
    public record CreatePermission
    {
        /// <summary>
        /// Optional user friendly description to create this permission with
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Required unique per scope name for this permission
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Required unique id of the scope this permission exists in
        /// </summary>
        public Guid ScopeId { get; set; }
    }
}
