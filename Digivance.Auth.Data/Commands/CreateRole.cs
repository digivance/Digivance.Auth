namespace Digivance.Auth.Data.Commands
{
    /// <summary>
    /// Command to create a new role
    /// </summary>
    public record CreateRole
    {
        /// <summary>
        /// User friendly description of the role to create
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Required unique per scope name of this role
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Optional collection of unique id's of permissions to assign to this role
        /// </summary>
        public ICollection<Guid>? PermissionIds { get; set; }

        /// <summary>
        /// Required, the unique if of the scope to create this role in
        /// </summary>
        public Guid ScopeId { get; set; }
    }
}
